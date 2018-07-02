using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Life_Web.Models;
using Models;

namespace Life_Web.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<User> User { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Node> Node { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Paragraph> Paragraph { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<ParagraphNode> ParagraphNodes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<UserCollection> UserCollections { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<UserMessage> UserMessages { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Comment> Comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<HtmlPackUrlInfo> HtmlPackUrlInfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Advertisement> Advertisements { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<GameStrategy> GameStrategy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<GameNode> GameNode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<GameHtmlInfo> GameHtmlInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
