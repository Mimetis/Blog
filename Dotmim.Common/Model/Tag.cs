using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dotmim.Common.Model
{
    public class Tag
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }
}
