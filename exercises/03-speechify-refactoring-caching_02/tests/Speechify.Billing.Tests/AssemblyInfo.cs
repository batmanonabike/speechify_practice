// The legacy engine keeps a static rate cache and a static clock hook, so test
// classes cannot safely run concurrently against it.
[assembly: CollectionBehavior(DisableTestParallelization = true)]
