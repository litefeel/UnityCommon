using System;

namespace Litefeel.UnityCommon
{
    public static class ObjectPool<T> where T : class, new()
    {
        // Object pool to avoid allocations.
        private static readonly InternalPool<T> s_Pool = new InternalPool<T>(null, null);

        public static T Get()
        {
            return s_Pool.Get();
        }

        public static void Release(T toRelease)
        {
            s_Pool.Release(toRelease);
        }
    }

    public struct ObjectPoolScope<T> : IDisposable where T : class, new()
    {
        private bool m_Disposed;
        public readonly T obj;
        private ObjectPoolScope(T obj)
        {
            m_Disposed = false;
            this.obj = obj;
        }
        public void Dispose()
        {
            if (m_Disposed)
                return;
            m_Disposed = true;
            ObjectPool<T>.Release(obj);
        }

        public static ObjectPoolScope<T> Create()
        {
            return new ObjectPoolScope<T>(ObjectPool<T>.Get());
        }
    }
}