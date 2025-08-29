using Secs.CodeGen;

var basePath = "/Users/artemsv/Projects/secs/Secs/";

var code = LambdaCodeBuilder.BuildCode(false, false, 8, out var name);
//File.WriteAllText(Path.Combine(basePath, name).Replace("\\", "/"), code);

code = LambdaCodeBuilder.BuildCode(false, true, 8, out name);
//File.WriteAllText(Path.Combine(basePath, name).Replace("\\", "/"), code);

code = LambdaCodeBuilder.BuildCode(true, false, 8, out name);
//File.WriteAllText(Path.Combine(basePath, name).Replace("\\", "/"), code);

code = LambdaCodeBuilder.BuildCode(true, true, 8, out name);
//File.WriteAllText(Path.Combine(basePath, name).Replace("\\", "/"), code);

code = IteratorCodeBuilder.BuildCode(false, 8, out name);
File.WriteAllText(Path.Combine(basePath, name).Replace("\\", "/"), code);

code = IteratorCodeBuilder.BuildCode(true, 8, out name);
File.WriteAllText(Path.Combine(basePath, name).Replace("\\", "/"), code);