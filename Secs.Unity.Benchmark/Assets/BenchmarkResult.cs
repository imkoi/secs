namespace DefaultNamespace
{
    public struct BenchmarkResult
    {
        public double CreateEntitiesTime;
        public double IterateOneComponentTime;
        public double IterateTwoComponentTime;
        public double IterateTwoComponent3Time;
        public double IterateThreeSparseComponent3Time;
        public int TotalIterations;
        public double CreateEntitiesWith3DenseComponents { get; set; }
        public double CreateEntitiesWith3SparseComponents { get; set; }
        public double CreateEntitiesWith2DenseComponents { get; set; }
        public double CreateEntitiesWith2SparseComponents { get; set; }
        public double CreateEntitiesWith1DenseComponents { get; set; }
        public double CreateEntitiesWith1SparseComponents { get; set; }
        
        public double DestroyEntitiesWith3DenseComponents { get; set; }
        public double DestroyEntitiesWith3SparseComponents { get; set; }
        public double DestroyEntitiesWith2DenseComponents { get; set; }
        public double DestroyEntitiesWith2SparseComponents { get; set; }
        public double DestroyEntitiesWith1DenseComponents { get; set; }
        public double DestroyEntitiesWith1SparseComponents { get; set; }
        
        public double AddNewComponentWith3SparseComponents { get; set; }
        public double AddNewComponentWith3DenseComponents { get; set; }
        public double AddNewComponentWith2SparseComponents { get; set; }
        public double AddNewComponentWith2DenseComponents { get; set; }
        public double AddNewComponentWith1SparseComponents { get; set; }
        public double AddNewComponentWith1DenseComponents { get; set; }
        
        public double IterateWith1DenseComponents { get; set; }
        public double IterateWith1SparseComponents { get; set; }
        public double IterateWith2DenseComponents { get; set; }
        public double IterateWith2SparseComponents { get; set; }
        public double IterateWith3DenseComponents { get; set; }
        public double IterateWith3SparseComponents { get; set; }
    }
}