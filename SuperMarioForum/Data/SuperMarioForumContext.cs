using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuperMarioForum.Models;

namespace SuperMarioForum.Data
{
    public class SuperMarioForumContext : DbContext
    {
        public SuperMarioForumContext (DbContextOptions<SuperMarioForumContext> options)
            : base(options)
        {
        }

        public DbSet<SuperMarioForum.Models.Discussion> Discussion { get; set; } = default!;
        public DbSet<SuperMarioForum.Models.Comment> Comment { get; set; } = default!;
    }
}
