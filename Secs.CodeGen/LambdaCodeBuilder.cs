namespace Secs.CodeGen;

public class LambdaCodeBuilder
{
    public static string BuildCode(bool mutable, bool withEntity, int methodsAmount, out string fileName)
    {
        var mutabilityName = mutable ? "Mutable" : "Immutable";
        var entityPostfix = withEntity ? "WithEntity" : "";
        var delegateName = $"Each{mutabilityName}{entityPostfix}";

        fileName = $"Registry.{delegateName}.cs";

        var codeWriter = new CodeWriter(4, "System", "System.Linq");

        using (codeWriter.Scope("namespace Secs"))
        {
            using (codeWriter.Scope("public partial class Registry"))
            {
                for (var i = 0; i < methodsAmount; i++)
                {
                    var genericParameters = new List<string>();

                    for (var j = 0; j < i + 1; j++)
                    {
                        genericParameters.Add($"T{j}");
                    }

                    var genericParameterLiteral = $"<{string.Join(", ", genericParameters)}>";

                    var delegateLiteral = $"{delegateName}{genericParameterLiteral}";

                    var variables = new List<string>();

                    variables.Add("Registry registry");

                    if (withEntity)
                    {
                        variables.Add("int entity");
                    }

                    for (var j = 0; j < i + 1; j++)
                    {
                        var parameter = genericParameters[j] + $" v{j}";

                        if (mutable)
                        {
                            parameter = "ref " + parameter;
                        }

                        variables.Add(parameter);
                    }

                    var whereOperators = new List<string>();

                    for (var j = 0; j < i + 1; j++)
                    {
                        whereOperators.Add($"           where T{j} : struct");
                    }

                    var methodName = withEntity ? "EachWithEntity" : "Each";

                    codeWriter.WriteLine(
                        $"public delegate void {delegateLiteral}({string.Join(", ", variables)})\n{string.Join("\n", whereOperators)};");
                    codeWriter.WriteEmptyLine();
                    codeWriter.WriteLine(
                        $"public unsafe void {methodName}{genericParameterLiteral}({delegateLiteral} each, Filter filter = default)");

                    var methodImplementationScope = default(IDisposable);

                    for (var j = 0; j < i + 1; j++)
                    {
                        var line = $"   where T{j} : struct";

                        if (j == i)
                        {
                            methodImplementationScope = codeWriter.Scope(line);
                        }
                        else
                        {
                            codeWriter.WriteLine(line);
                        }
                    }

                    using (methodImplementationScope)
                    {
                        for (var j = 0; j < i + 1; j++)
                        {
                            using (codeWriter.Scope($"if (!_componentIds.TryGetValue(typeof(T{j}), out var id{j}))"))
                            {
                                codeWriter.WriteLine("return;");
                            }

                            codeWriter.WriteLine($"var components{j} = _components[id{j}];");
                            codeWriter.WriteLine($"var sparse{j} = components{j}._sparse;");
                            codeWriter.WriteLine($"var dense{j} = components{j}.GetDense<T{j}>();");

                            codeWriter.WriteEmptyLine();
                        }

                        codeWriter.WriteLine("var include = filter.Include;");
                        codeWriter.WriteLine("var exclude = filter.Exclude;");
                        codeWriter.WriteLine($"var includesCount = include?.Length + {i + 1} ??  {i + 1};");
                        codeWriter.WriteLine($"var componentIdsCount = includesCount + (exclude?.Length ?? 0);");
                        codeWriter.WriteLine($"var componentIds = stackalloc int[componentIdsCount];");
                        codeWriter.WriteEmptyLine();

                        for (var j = 0; j < i + 1; j++)
                        {
                            codeWriter.WriteLine($"componentIds[{j}] = id{j};");
                        }

                        codeWriter.WriteEmptyLine();

                        using (codeWriter.Scope("if (include != null)"))
                        {
                            codeWriter.WriteLine($"FillComponentIds(include, componentIds, {i + 1});");
                        }

                        codeWriter.WriteLine("QuickSort(componentIds, 0, includesCount - 1, 1);");

                        using (codeWriter.Scope("if (exclude != null)"))
                        {
                            codeWriter.WriteLine($"FillComponentIds(exclude, componentIds, includesCount);");
                            codeWriter.WriteLine("QuickSort(componentIds, includesCount, componentIdsCount - 1, -1);");
                        }

                        codeWriter.WriteLine(
                            "var compactIncludeId = FindCompactIncludeId(componentIds, includesCount);");
                        codeWriter.WriteEmptyLine();
                        codeWriter.WriteLine("includesCount--;");
                        codeWriter.WriteEmptyLine();
                        codeWriter.WriteLine("var components = _components[compactIncludeId];");
                        codeWriter.WriteLine("var entities = components._entities;");
                        codeWriter.WriteLine("var finalFilterCount = componentIdsCount - 1;");
                        codeWriter.WriteLine("var sparses = stackalloc int*[finalFilterCount];");
                        codeWriter.WriteEmptyLine();
                        codeWriter.WriteLine("var x = 0;");
                        codeWriter.WriteEmptyLine();

                        using (codeWriter.Scope("for (var i = 0; i < componentIdsCount; ++i)"))
                        {
                            using (codeWriter.Scope("if (componentIds[i] == compactIncludeId)"))
                            {
                                codeWriter.WriteLine("continue;");
                            }

                            using (codeWriter.Scope(
                                       "fixed (int* sparsePointer = _components[componentIds[i]]._sparse)"))
                            {
                                codeWriter.WriteLine("sparses[x] = sparsePointer;");
                            }

                            codeWriter.WriteLine("x++;");
                        }

                        codeWriter.WriteLine("var match = true;");
                        codeWriter.WriteLine("var entity = -1;");
                        codeWriter.WriteLine("var entityIndex = components._count - 1;");
                        codeWriter.WriteLine("var componentIndex = 0;");
                        codeWriter.WriteEmptyLine();

                        codeWriter.WriteLine("MoveNextEntity:");
                        using (codeWriter.Scope("if (entityIndex >= 0)"))
                        {
                            codeWriter.WriteLine("entity = entities[entityIndex];");
                            codeWriter.WriteLine("match = true;");
                            codeWriter.WriteEmptyLine();
                            codeWriter.WriteLine("MoveNextComponent:");

                            using (codeWriter.Scope("if (match && componentIndex < finalFilterCount)"))
                            {
                                codeWriter.WriteLine(
                                    "match = sparses[componentIndex][entity] > 0 == componentIndex < includesCount;");
                                codeWriter.WriteEmptyLine();

                                codeWriter.WriteLine("componentIndex++;");
                                codeWriter.WriteLine("goto MoveNextComponent;");
                            }

                            using (codeWriter.Scope("if (match)"))
                            {
                                var arguments = new List<string>();

                                if (withEntity)
                                {
                                    arguments.Add("entity");
                                }

                                var refLiteral = mutable ? "ref " : string.Empty;

                                for (var j = 0; j < i + 1; j++)
                                {
                                    arguments.Add($"{refLiteral}dense{j}[sparse{j}[entity] - 1]");
                                }

                                codeWriter.WriteLine($"each(this, {string.Join(", ", arguments)});");
                            }

                            codeWriter.WriteLine("componentIndex = 0;");
                            codeWriter.WriteLine("entityIndex--;");
                            codeWriter.WriteLine("goto MoveNextEntity;");
                        }
                    }
                }
            }
        }

        return codeWriter.Build();
    }
}