using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGQuoye
{
    public class QuoueItem<T>
    {
        public int Id { get; set; } = 0;
        public string JournalNum { get; set; } = "";
        public string UserId { get; set; } = "";
        public Queue<T> PostingQ { get; set; } = new Queue<T>();
        public void Pull()
        {
            if (PostingQ.Count == 0)
                return;

          var r=  PostingQ.Dequeue();
        }
    }
}
