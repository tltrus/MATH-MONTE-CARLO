using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Percolation
{
    public class WQUPC
    {
        public int[] id, sz;

        //Weighted Quick Union With Path Compression
        public WQUPC(int n)
        {
            id = new int[n];
            sz = new int[n];
            for (int i = 0; i < n; i++)
            {
                id[i] = i;
                sz[i] = 1;
            }
        }

        public int Root(int x)
        {
            while (x != id[x])
            {
                id[x] = id[id[x]];
                x = id[x];
            }

            return x;
        }

        public bool isConnected(int p, int q)
        {
            return Root(p) == Root(q);
        }

        public void Union(int p, int q)
        {
            int i = Root(p);
            int j = Root(q);
            if (i == j) return;

            if (sz[i] < sz[j])
            {
                id[i] = id[j];
                sz[j] += sz[i];
            }
            else
            {
                id[j] = id[i];
                sz[i] += sz[j];
            }
        }
    }
}
