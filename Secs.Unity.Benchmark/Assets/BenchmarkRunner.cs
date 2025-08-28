using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using DefaultNamespace;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Scripting;

public class BenchmarkRunner : MonoBehaviour
{
    public bool _testLeo;
    public bool _testMorpeh;
    public bool _testSecs;
    
    private ProfilerRecorder _totalUsed;
    private ProfilerRecorder _gcUsed;
    
    public delegate void ScenarioDelegate(int entity, Stopwatch stopwatch, ref BenchmarkResult result);

    private void Awake()
    {
        GarbageCollector.GCMode = GarbageCollector.Mode.Manual;
        GarbageCollector.CollectIncremental(500_000_000);
        
        _totalUsed = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Used Memory");
        _gcUsed    = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Used Memory");
    }

    private void OnDestroy()
    {
        _totalUsed.Dispose();
        _gcUsed.Dispose();
    }

    void MeasureLeo()
    {
        var sw = new Stopwatch();

        var samplesCount = 1;

        var results = new BenchmarkResult[3];
        var iterationsList = new List<int> { 10_000, 100_000, 1_000_000 };
        var leoScenarios = LeoBenchmark.GetScenarios();

        var idx = 0;

        foreach (var iterations in iterationsList)
        {
            ref var result = ref results[idx++];
            
            for (var i = 0; i < samplesCount; i++)
            {
                foreach (var pair in leoScenarios)
                {
                    pair.Value.Invoke(iterations, sw, ref result);
                }
            }
        }

        for (var i = 0; i < 3; i++)
        {
            Log("Leo", samplesCount, iterationsList[i], results[i]);
        }
    }
    
    void MeasureMorpeh()
    {
        var sw = new Stopwatch();

        var samplesCount = 1;

        var results = new BenchmarkResult[3];
        var iterationsList = new List<int> { 10_000, 100_000, 1_000_000 };
        var morpehScenarios = MorpehBenchmark.GetScenarios();
        
        var idx = 0;

        foreach (var iterations in iterationsList)
        {
            ref var result = ref results[idx++];
            
            for (var i = 0; i < samplesCount; i++)
            {
                foreach (var pair in morpehScenarios)
                {
                    pair.Value.Invoke(iterations, sw, ref result);
                }
            }
        }

        for (var i = 0; i < 3; i++)
        {
            Log("Morpeh", samplesCount, iterationsList[i], results[i]);
        }
    }
    
    void MeasureSecs()
    {
        var sw = new Stopwatch();

        var samplesCount = 1;
        
        var results = new BenchmarkResult[3];
        var iterationsList = new List<int> { 10_000, 100_000, 1_000_000 };
        var secsScenarios = SecsBenchmark.GetScenarios();
        
        var idx = 0;

        foreach (var iterations in iterationsList)
        {
            ref var result = ref results[idx++];
            
            for (var i = 0; i < samplesCount; i++)
            {
                foreach (var pair in secsScenarios)
                {
                    pair.Value.Invoke(iterations, sw, ref result);
                }
            }
        }

        for (var i = 0; i < 3; i++)
        {
            Log("Secs", samplesCount, iterationsList[i], results[i]);
        }
    }

    private void Log(string frameworkName, int samplesCount, int iterations, BenchmarkResult result)
    {
        var sb = new StringBuilder();

        result.CreateEntitiesWith3DenseComponents /= samplesCount * 1f;
        result.CreateEntitiesWith3SparseComponents /= samplesCount * 1f;
        result.CreateEntitiesWith2DenseComponents /= samplesCount * 1f;
        result.CreateEntitiesWith2SparseComponents /= samplesCount * 1f;
        result.CreateEntitiesWith1DenseComponents /= samplesCount * 1f;
        result.CreateEntitiesWith1SparseComponents /= samplesCount * 1f;

        result.DestroyEntitiesWith3DenseComponents /= samplesCount * 1f;
        result.DestroyEntitiesWith3SparseComponents /= samplesCount * 1f;
        result.DestroyEntitiesWith2DenseComponents /= samplesCount * 1f;
        result.DestroyEntitiesWith2SparseComponents /= samplesCount * 1f;
        result.DestroyEntitiesWith1DenseComponents /= samplesCount * 1f;
        result.DestroyEntitiesWith1SparseComponents /= samplesCount * 1f;

        result.AddNewComponentWith3SparseComponents /= samplesCount * 1f;
        result.AddNewComponentWith3DenseComponents /= samplesCount * 1f;
        result.AddNewComponentWith2SparseComponents /= samplesCount * 1f;
        result.AddNewComponentWith2DenseComponents /= samplesCount * 1f;
        result.AddNewComponentWith1SparseComponents /= samplesCount * 1f;
        result.AddNewComponentWith1DenseComponents /= samplesCount * 1f;

        result.IterateWith1DenseComponents /= samplesCount * 1f;
        result.IterateWith1SparseComponents /= samplesCount * 1f;
        result.IterateWith2DenseComponents /= samplesCount * 1f;
        result.IterateWith2SparseComponents /= samplesCount * 1f;
        result.IterateWith3DenseComponents /= samplesCount * 1f;
        result.IterateWith3SparseComponents /= samplesCount * 1f;

        sb.AppendLine($"{frameworkName}[{iterations}]:");
        
        sb.AppendLine($"TOTAL: {_totalUsed.LastValue/1048576f:f2} MB");
        sb.AppendLine($"GC: {_gcUsed.LastValue/1048576f:f2} MB");
        
        sb.Append($"{nameof(result.CreateEntitiesWith1DenseComponents)}: ")
            .AppendLine($"{result.CreateEntitiesWith1DenseComponents:f2} milliseconds");
        sb.Append($"{nameof(result.CreateEntitiesWith1SparseComponents)}: ")
            .AppendLine($"{result.CreateEntitiesWith1SparseComponents:f2} milliseconds");
        sb.Append($"{nameof(result.CreateEntitiesWith2DenseComponents)}: ")
            .AppendLine($"{result.CreateEntitiesWith2DenseComponents:f2} milliseconds");
        sb.Append($"{nameof(result.CreateEntitiesWith2SparseComponents)}: ")
            .AppendLine($"{result.CreateEntitiesWith2SparseComponents:f2} milliseconds");
        sb.Append($"{nameof(result.CreateEntitiesWith3DenseComponents)}: ")
            .AppendLine($"{result.CreateEntitiesWith3DenseComponents:f2} milliseconds");
        sb.Append($"{nameof(result.CreateEntitiesWith3SparseComponents)}: ")
            .AppendLine($"{result.CreateEntitiesWith3SparseComponents:f2} milliseconds");
        sb.AppendLine();

        sb.Append($"{nameof(result.DestroyEntitiesWith1DenseComponents)}: ")
            .AppendLine($"{result.DestroyEntitiesWith1DenseComponents:f2} milliseconds");
        sb.Append($"{nameof(result.DestroyEntitiesWith1SparseComponents)}: ")
            .AppendLine($"{result.DestroyEntitiesWith1SparseComponents:f2} milliseconds");
        sb.Append($"{nameof(result.DestroyEntitiesWith2DenseComponents)}: ")
            .AppendLine($"{result.DestroyEntitiesWith2DenseComponents:f2} milliseconds");
        sb.Append($"{nameof(result.DestroyEntitiesWith2SparseComponents)}: ")
            .AppendLine($"{result.DestroyEntitiesWith2SparseComponents:f2} milliseconds");
        sb.Append($"{nameof(result.DestroyEntitiesWith3DenseComponents)}: ")
            .AppendLine($"{result.DestroyEntitiesWith3DenseComponents:f2} milliseconds");
        sb.Append($"{nameof(result.DestroyEntitiesWith3SparseComponents)}: ")
            .AppendLine($"{result.DestroyEntitiesWith3SparseComponents:f2} milliseconds");
        sb.AppendLine();

        sb.Append($"{nameof(result.AddNewComponentWith1DenseComponents)}: ")
            .AppendLine($"{result.AddNewComponentWith1DenseComponents:f2} milliseconds");
        sb.Append($"{nameof(result.AddNewComponentWith1SparseComponents)}: ")
            .AppendLine($"{result.AddNewComponentWith1SparseComponents:f2} milliseconds");
        sb.Append($"{nameof(result.AddNewComponentWith2DenseComponents)}: ")
            .AppendLine($"{result.AddNewComponentWith2DenseComponents:f2} milliseconds");
        sb.Append($"{nameof(result.AddNewComponentWith2SparseComponents)}: ")
            .AppendLine($"{result.AddNewComponentWith2SparseComponents:f2} milliseconds");
        sb.Append($"{nameof(result.AddNewComponentWith3DenseComponents)}: ")
            .AppendLine($"{result.AddNewComponentWith3DenseComponents:f2} milliseconds");
        sb.Append($"{nameof(result.AddNewComponentWith3SparseComponents)}: ")
            .AppendLine($"{result.AddNewComponentWith3SparseComponents:f2} milliseconds");
        sb.AppendLine();

        sb.Append($"{nameof(result.IterateWith1DenseComponents)}: ")
            .AppendLine($"{result.IterateWith1DenseComponents:f2} milliseconds");
        sb.Append($"{nameof(result.IterateWith1SparseComponents)}: ")
            .AppendLine($"{result.IterateWith1SparseComponents:f2} milliseconds");
        sb.Append($"{nameof(result.IterateWith2DenseComponents)}: ")
            .AppendLine($"{result.IterateWith2DenseComponents:f2} milliseconds");
        sb.Append($"{nameof(result.IterateWith2SparseComponents)}: ")
            .AppendLine($"{result.IterateWith2SparseComponents:f2} milliseconds");
        sb.Append($"{nameof(result.IterateWith3DenseComponents)}: ")
            .AppendLine($"{result.IterateWith3DenseComponents:f2} milliseconds");
        sb.Append($"{nameof(result.IterateWith3SparseComponents)}: ")
            .AppendLine($"{result.IterateWith3SparseComponents:f2} milliseconds");
        sb.AppendLine();

        var path = Path.Combine(Application.streamingAssetsPath, frameworkName + $"[{iterations}].txt").Replace("\\", "/");
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
        }

        File.WriteAllText(path, sb.ToString());
    }

    private int frame = 4;

    private void Update()
    {
        frame--;

        if (frame == 0)
        {
            if (_testLeo)
            {
                MeasureLeo();
            }

            if (_testMorpeh)
            {
                MeasureMorpeh();
            }

            if (_testSecs)
            {
                MeasureSecs();
            }
            
            Application.Quit();
        }
    }
}