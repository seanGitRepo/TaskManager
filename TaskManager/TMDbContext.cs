using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace TaskManager
{
    internal class TMDbContext: DbContext
    {
        public DbSet<Task> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder ob)
        {
            base.OnConfiguring(ob);
            string cs = "Data Source = DESKTOP-UUNQOH5;Initial Catalog = TaskManager; Integrated Security = True;Trust Server Certificate=True";
            //server                        //database                  //how do we authenticate      //is the minimum that we need.                          

            ob.UseSqlServer(cs);//catalog is the database.

        }



    }
}
