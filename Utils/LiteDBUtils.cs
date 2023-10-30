using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using MainProject.Bean;

namespace MainProject.Utils
{
    public class LiteDBUtils
    {

        public static int insertData(ProgramSeq ProgramSeq)
        {
            int id = 0;
            using (LiteDatabase db = new LiteDatabase(@"ParametricDesign.db"))
            {
                // Get customer collection
                var col = db.GetCollection<ProgramSeq>("MainSeq");

                // Create unique index in Name field
                col.EnsureIndex(x => x.id, true);

                // Insert new customer document (Id will be auto-incremented)
                id = col.Insert(ProgramSeq);
                // Update a document inside a collection
                //customer.Name = "Joana Doe";

                //col.Update(customer);

                // Use LINQ to query documents (with no index)
                //var results = col.Find(x => x.Age > 20);
            }
            return id;
        }

        public static void deleteData(int id)
        {
            using (LiteDatabase db = new LiteDatabase(@"ParametricDesign.db"))
            {
                // Get customer collection
                var col = db.GetCollection<ProgramSeq>("MainSeq");
                //col.EnsureIndex(x => x.SubjectName, true);
                // Create unique index in Name field

                //ProgramSeq project = col.FindOne(x => x.SubjectName == subjectName);
               
                // Insert new customer document (Id will be auto-incremented)
                col.Delete(id);
                // Update a document inside a collection
                //customer.Name = "Joana Doe";

                //col.Update(customer);

                // Use LINQ to query documents (with no index)
                //var results = col.Find(x => x.Age > 20);
            }
        }

        public static ProgramSeq queryOne(int id)
        {
            using (LiteDatabase db = new LiteDatabase(@"ParametricDesign.db"))
            {
                // Get customer collection
                var col = db.GetCollection<ProgramSeq>("MainSeq");
                //col.EnsureIndex(x => x.SubjectName, true);
                // Create unique index in Name field

                ProgramSeq project = col.FindById(id);

                return project;
                // Insert new customer document (Id will be auto-incremented)
                //col.Delete(project.id);
                // Update a document inside a collection
                //customer.Name = "Joana Doe";

                //col.Update(customer);

                // Use LINQ to query documents (with no index)
                //var results = col.Find(x => x.Age > 20);
            }
        }

        public static List<ProgramSeq> queryAll()
        {
            using (LiteDatabase db = new LiteDatabase(@"ParametricDesign.db"))
            {
                // Get customer collection
                var col = db.GetCollection<ProgramSeq>("MainSeq");
                //col.EnsureIndex(x => x.SubjectName, true);
                // Create unique index in Name field

                List<ProgramSeq> projects = col.FindAll().ToList();

                return projects;
                // Insert new customer document (Id will be auto-incremented)
                //col.Delete(project.id);
                // Update a document inside a collection
                //customer.Name = "Joana Doe";

                //col.Update(customer);

                // Use LINQ to query documents (with no index)
                //var results = col.Find(x => x.Age > 20);
            }
        }

        public static void updateProgramSeq(ProgramSeq newproject)
        {
            using (LiteDatabase db = new LiteDatabase(@"ParametricDesign.db"))
            {
                // Get customer collection
                var col = db.GetCollection<ProgramSeq>("MainSeq");
                //col.EnsureIndex(x => x.SubjectName, true);
                // Create unique index in Name field

                //ProgramSeq oldproject = col.FindById(newproject.id);
                //oldproject.
                //if (oldproject != null)
                {
                    col.Update(newproject);
                }
                // Insert new customer document (Id will be auto-incremented)
                //col.Delete(project.id);
                // Update a document inside a collection
                //customer.Name = "Joana Doe";

                //col.Update(customer);

                // Use LINQ to query documents (with no index)
                //var results = col.Find(x => x.Age > 20);
            }
        }
    }

    
}
