namespace Secs.CodeGen;

public class IteratorCodeBuilder
{
    public static string BuildCode(bool withEntity, int methodsAmount, out string fileName)
    {
        var entityPostfix = withEntity ? "WithEntity" : "";
        var methodName = $"ComponentIterator{entityPostfix}";

        fileName = $"Registry.{methodName}.cs";

        var codeWriter = new CodeWriter(4, "System.Runtime.CompilerServices");

        using (codeWriter.Scope("namespace Ecs"))
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

                    var structureLiteral = $"{methodName}{genericParameterLiteral}";

                    var whereOperators = new List<string>();

                    for (var j = 0; j < i + 1; j++)
                    {
                        whereOperators.Add($"\n           where T{j} : struct");
                    }
                    
                    var methodNamePostfix = withEntity ? "WithEntity" : "";

                    using (codeWriter.Scope(
                               $"public {structureLiteral} Each{methodNamePostfix}{genericParameterLiteral}(Filter filter = default){string.Join(' ', whereOperators)}"))
                    {
                        codeWriter.WriteLine($"return new {structureLiteral}(this, filter);");
                    }

                    codeWriter.WriteLine(
                        $"public unsafe struct {methodName}{genericParameterLiteral}");

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
                        var argAmount = i + 1;
                        var args = new List<string>(argAmount);

                        if (withEntity)
                        {
                            args.Add("int entityId");
                        }
                        
                        for (var j = 0; j < argAmount; j++)
                        {
                            var line = $"Ref<T{j}>";

                            args.Add(line);
                        }

                        var currentType = args.Count == 1 ? "Ref<T0>" : $"({string.Join(", ", args)})";

                        codeWriter.WriteLine("private readonly int _finalFilterCount;");
                        codeWriter.WriteLine("private readonly int _includesCount;");
                        codeWriter.WriteLine("private readonly int[] _entities;");
                        codeWriter.WriteLine("private readonly int[][] _sparses;");

                        codeWriter.WriteEmptyLine();

                        codeWriter.WriteLine("private int _entityIndex;");
                        codeWriter.WriteLine($"public {currentType} Current " + "{ get; set; }");

                        codeWriter.WriteEmptyLine();

                        codeWriter.WriteLine($"private {currentType} _current;");

                        codeWriter.WriteEmptyLine();

                        using (codeWriter.Scope($"public {methodName}(Registry registry, Filter filter)"))
                        {
                            codeWriter.WriteLine("var registryComponentIds = registry._componentIds;");
                            codeWriter.WriteLine("var registryComponents = registry._components;");
                            codeWriter.WriteEmptyLine();
                            codeWriter.WriteLine("var include = filter.Include;");
                            codeWriter.WriteLine("var exclude = filter.Exclude;");
                            codeWriter.WriteLine($"var includesCount = include?.Length + {i + 1} ??  {i + 1};");
                            codeWriter.WriteLine("var componentIdsCount = includesCount + (exclude?.Length ?? 0);");
                            codeWriter.WriteLine("var componentIds = stackalloc int[componentIdsCount];");
                            codeWriter.WriteEmptyLine();

                            for (var j = 0; j < i + 1; j++)
                            {
                                codeWriter.WriteLine($"var sparse{j} = default(int[]);");
                                codeWriter.WriteLine($"var dense{j} = default(T{j}*);");
                                codeWriter.WriteLine($"var id{j} = -1;");
                            }
                            
                            var conditionElements = new List<string>();
                            
                            for (var j = 0; j < i + 1; j++)
                            {
                                conditionElements.Add($"registryComponentIds.TryGetValue(typeof(T{j}), out id{j})");
                            }
                            
                            codeWriter.WriteLine($"var hasAllQueryComponents = {string.Join(" && ", conditionElements)};");

                            using (codeWriter.Scope("if (hasAllQueryComponents)"))
                            {
                                for (var j = 0; j < i + 1; j++)
                                {
                                    codeWriter.WriteLine($"var components{j} = registryComponents[id{j}];");
                                    codeWriter.WriteLine($"sparse{j} = components{j}._sparse;");
                                    codeWriter.WriteLine($"dense{j} = components{j}.GetDense<T{j}>();");
                                    
                                    codeWriter.WriteLine($"componentIds[{j}] = id{j};");

                                    codeWriter.WriteEmptyLine();
                                }
                            }
                            
                            using (codeWriter.Scope("if (include != null)"))
                            {
                                codeWriter.WriteLine($"registry.FillComponentIds(include, componentIds, {i + 1});");
                            }

                            codeWriter.WriteLine("QuickSort(componentIds, 0, includesCount - 1, 1);");

                            using (codeWriter.Scope("if (exclude != null)"))
                            {
                                codeWriter.WriteLine("registry.FillComponentIds(exclude, componentIds, includesCount);");
                                codeWriter.WriteLine(
                                    "QuickSort(componentIds, includesCount, componentIdsCount - 1, -1);");
                            }

                            codeWriter.WriteLine(
                                "var compactIncludeId = registry.FindCompactIncludeId(componentIds, includesCount);");
                            codeWriter.WriteEmptyLine();
                            codeWriter.WriteLine("var components = registryComponents[compactIncludeId];");
                            codeWriter.WriteLine("var entities = components._entities;");
                            codeWriter.WriteLine("var finalFilterCount = componentIdsCount - 1;");
                            
                            codeWriter.WriteLine("var sparses = _stack.Count > 0 ? _stack.Pop() : new int[64][];");
                            codeWriter.WriteEmptyLine();
                            codeWriter.WriteLine("var x = 0;");
                            codeWriter.WriteEmptyLine();

                            using (codeWriter.Scope("for (var i = 0; i < componentIdsCount; ++i)"))
                            {
                                using (codeWriter.Scope("if (componentIds[i] == compactIncludeId)"))
                                {
                                    codeWriter.WriteLine("continue;");
                                }
                                
                                codeWriter.WriteLine("sparses[x] = registryComponents[componentIds[i]]._sparse;");
                                codeWriter.WriteLine("x++;");
                            }
                            
                            codeWriter.WriteLine("_entities = entities;");
                            codeWriter.WriteLine("_sparses = sparses;");
                            codeWriter.WriteLine("_entityIndex = components._count - 1;");
                            codeWriter.WriteLine("_includesCount = includesCount - 1;");
                            codeWriter.WriteLine("_finalFilterCount = finalFilterCount;");
                            
                            args.Clear();

                            if (withEntity)
                            {
                                args.Add("-1");
                            }
                        
                            for (var j = 0; j < argAmount; j++)
                            {
                                var line = $"new Ref<T{j}>" + "{ " + $"_sparse = sparse{j}, _dense = dense{j}," + " }";

                                args.Add(line);
                            }

                            if (args.Count == 0)
                            {
                                var line = "new Ref<T0>" + "{ " + "_sparse = sparse0, _dense = dense0," + " }";
                                
                                args.Add(line);
                            }

                            var newCurrent = args.Count == 1 
                                ? args.First() 
                                : $"({string.Join(", ", args)})";

                            codeWriter.WriteLine($"_current = {newCurrent};");
                            
                            codeWriter.WriteLine("Current = _current;");
                        }
                        
                        codeWriter.WriteLine($"public {structureLiteral} GetEnumerator() => this;");
                        codeWriter.WriteEmptyLine();
                        
                        codeWriter.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                        using (codeWriter.Scope("public bool MoveNext()"))
                        {
                            codeWriter.WriteLine("var match = true;");
                            codeWriter.WriteLine("var entity = -1;");
                            codeWriter.WriteLine("var entityIndex = _entityIndex;");
                            codeWriter.WriteLine("var sparses = _sparses;");
                            codeWriter.WriteLine("var includesCount = _includesCount;");
                            codeWriter.WriteLine("var finalFilterCount = _finalFilterCount;");
                            codeWriter.WriteLine("var entities = _entities;");
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
                                
                                codeWriter.WriteLine("componentIndex = 0;");
                                codeWriter.WriteLine("entityIndex--;");
                                
                                codeWriter.WriteEmptyLine();

                                using (codeWriter.Scope("if (!match)"))
                                {
                                    codeWriter.WriteLine("goto MoveNextEntity;");
                                }
                            }
                            
                            codeWriter.WriteLine("_entityIndex = entityIndex;");
                            
                            codeWriter.WriteEmptyLine();

                            if (args.Count > 1)
                            {
                                var itemIdx = 1;
                                
                                if (withEntity)
                                {
                                    itemIdx++;
                                    
                                    codeWriter.WriteLine("_current.entityId = entity;");

                                    while (itemIdx < argAmount)
                                    {
                                        codeWriter.WriteLine($"_current.Item{itemIdx}._entity = entity;");
                                        
                                        itemIdx++;
                                    }
                                }
                            }
                            else
                            {
                                codeWriter.WriteLine("_current._entity = entity;");
                            }
                            
                            codeWriter.WriteEmptyLine();
                            
                            codeWriter.WriteLine("Current = _current;");
                            
                            codeWriter.WriteEmptyLine();

                            using (codeWriter.Scope("if (entity >= 0)"))
                            {
                                codeWriter.WriteLine("return true;");
                            }
                            
                            codeWriter.WriteLine("Dispose();");
                            codeWriter.WriteLine("return false;");
                        }

                        using (codeWriter.Scope("private void Dispose()"))
                        {
                            using (codeWriter.Scope("for (var i = 0; i < _sparses.Length; ++i)"))
                            {
                                using (codeWriter.Scope("if (_sparses[i] != null)"))
                                {
                                    codeWriter.WriteLine("_sparses[i] = null;");
                                }
                                using (codeWriter.Scope("else"))
                                {
                                    codeWriter.WriteLine("break;");
                                }
                            }
                            
                            codeWriter.WriteLine("_stack.Push(_sparses);");
                        }
                    }
                }
            }
        }

        return codeWriter.Build();
    }
}