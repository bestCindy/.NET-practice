using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    class test
    {
        static void Main(string[] args)
        {
             localdbEntities db = new localdbEntities();
            var model = db.allowance_summary.Select(m => new
            {
                ID = m.PERSON_ID
            });
            //var model = db.Classes.OrderBy(m => m.ID).Select(m => new
            //{
            //     ID = m.ID,
            //     Name = m.Name
            //});
 
             
             if(model.Count() > 0)
             {
                 Console.WriteLine("ID:", model.First().ID);
             }
         }
    }
}
