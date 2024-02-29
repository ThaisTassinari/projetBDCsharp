using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Data
{
    internal class Connect
    {
        private static String cliComConnectionString = GetConnectString();

        internal static String ConnectionString { get => cliComConnectionString; }

        private static String GetConnectString()
        {
            SqlConnectionStringBuilder cs = new SqlConnectionStringBuilder();
            cs.DataSource = "(local)";
            cs.InitialCatalog = "College1en";
            cs.UserID = "sa";
            cs.Password = "sysadm";
            return cs.ConnectionString;
        }
    }

    internal class DataTables
    {
        private static SqlDataAdapter adapterStudent = InitAdapterStudent();
        private static SqlDataAdapter adapterProg = InitAdapterProg();
        private static SqlDataAdapter adapterCours = InitAdapterCours();
        private static SqlDataAdapter adapterEnrollment = InitAdapterEnrollment();

        private static DataSet ds = InitDataSet();

        private static SqlDataAdapter InitAdapterStudent()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "SELECT * FROM Students ORDER BY StId ",
                Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            builder.ConflictOption = ConflictOption.OverwriteChanges;
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;
        }

        private static SqlDataAdapter InitAdapterProg()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "SELECT * FROM Programs ORDER BY ProgId ",
                Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;
        }

        private static SqlDataAdapter InitAdapterCours()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "SELECT * FROM Courses ORDER BY CId ",
                Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            builder.ConflictOption = ConflictOption.OverwriteChanges;
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;
        }

        private static SqlDataAdapter InitAdapterEnrollment()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "SELECT * FROM Enrollments ORDER BY StId, CId ",
                Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            builder.ConflictOption = ConflictOption.OverwriteChanges;
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;
        }

        private static DataSet InitDataSet()
        {
            DataSet ds = new DataSet();
            loadProg(ds);
            loadCours(ds);
            loadStudent(ds);
            loadEnrollment(ds);
            return ds;
        }

        private static void loadProg(DataSet ds)
        {
            //adapterProg.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            adapterProg.Fill(ds, "Programs");

            ds.Tables["Programs"].Columns["ProgId"].AllowDBNull = false;
            ds.Tables["Programs"].Columns["ProgName"].AllowDBNull = false;

            ds.Tables["Programs"].PrimaryKey = new DataColumn[1]
                    { ds.Tables["Programs"].Columns["ProgId"]};
   
        }

        private static void loadCours(DataSet ds)
        {
            //adapterCours.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            adapterCours.Fill(ds, "Courses");

            ds.Tables["Courses"].Columns["CId"].AllowDBNull = false;
            ds.Tables["Courses"].Columns["CName"].AllowDBNull = false;
            ds.Tables["Courses"].Columns["ProgId"].AllowDBNull = false;

            ds.Tables["Courses"].PrimaryKey = new DataColumn[1]
                  { ds.Tables["Courses"].Columns["CId"]};

            // verificar students tbm
            ForeignKeyConstraint myFK03 = new ForeignKeyConstraint("MyFK03",
                new DataColumn[]{
                    ds.Tables["Programs"].Columns["ProgId"]
                },
                new DataColumn[] {
                    ds.Tables["Courses"].Columns["ProgId"],
                }
            );
            myFK03.DeleteRule = Rule.Cascade;
            myFK03.UpdateRule = Rule.Cascade;
            ds.Tables["Courses"].Constraints.Add(myFK03);
        }

        private static void loadStudent(DataSet ds)
        {
            //adapterStudent.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            adapterStudent.Fill(ds, "Students");

            ds.Tables["Students"].Columns["StId"].AllowDBNull = false;
            ds.Tables["Students"].Columns["StName"].AllowDBNull = false;
            ds.Tables["Students"].Columns["ProgId"].AllowDBNull = false;

            ds.Tables["Students"].PrimaryKey = new DataColumn[1]
                  { ds.Tables["Students"].Columns["StId"]};

            ForeignKeyConstraint myFK04 = new ForeignKeyConstraint("MyFK04",
                new DataColumn[]{
                    ds.Tables["Programs"].Columns["ProgId"]
                },
                new DataColumn[] {
                    ds.Tables["Students"].Columns["ProgId"],
                }
            );
            myFK04.DeleteRule = Rule.None;
            myFK04.UpdateRule = Rule.Cascade;
            ds.Tables["Students"].Constraints.Add(myFK04);
        }

        private static void loadEnrollment(DataSet ds)
        {
            //adapterEnrollment.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            adapterEnrollment.Fill(ds, "Enrollments");

            ds.Tables["Enrollments"].Columns["StId"].AllowDBNull = false;
            ds.Tables["Enrollments"].Columns["CId"].AllowDBNull = false;

            ds.Tables["Enrollments"].PrimaryKey = new DataColumn[2]
                  { ds.Tables["Enrollments"].Columns["StId"], ds.Tables["Enrollments"].Columns["CId"] };


            /* Foreign Key between DataTables */

            ForeignKeyConstraint myFK01 = new ForeignKeyConstraint("MyFK01",
                new DataColumn[]{
                    ds.Tables["Students"].Columns["StId"]
                },
                new DataColumn[] {
                    ds.Tables["Enrollments"].Columns["StId"],
                }
            );
            myFK01.DeleteRule = Rule.Cascade;
            myFK01.UpdateRule = Rule.Cascade;
            ds.Tables["Enrollments"].Constraints.Add(myFK01);

            ForeignKeyConstraint myFK02 = new ForeignKeyConstraint("MyFK02",
              new DataColumn[]{
                    ds.Tables["Courses"].Columns["CId"]
              },
              new DataColumn[] {
                    ds.Tables["Enrollments"].Columns["CId"],
              }
          );
            myFK02.DeleteRule = Rule.None;
            myFK02.UpdateRule = Rule.None;
            ds.Tables["Enrollments"].Constraints.Add(myFK02);
 
        }

        internal static SqlDataAdapter getAdapterStudent()
        {
            return adapterStudent;
        }
        internal static SqlDataAdapter getAdapterProg()
        {
            return adapterProg;
        }

        internal static SqlDataAdapter getAdapterCours()
        {
            return adapterCours;
        }

        internal static SqlDataAdapter getAdapterEnrollment()
        {
            return adapterEnrollment;
        }
        internal static DataSet getDataSet()
        {
            return ds;
        }
    }

    internal class Students
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterStudent();
        private static DataSet ds = DataTables.getDataSet();

        internal static DataTable GetStudents()
        {
            return ds.Tables["Students"];
        }

        internal static int UpdateStudents()
        {
             try
             {
                 if (!ds.Tables["Students"].HasErrors)
                 {
                     return adapter.Update(ds.Tables["Students"]);

                 }
                 return 0;
             }
             catch (Exception)
             {
                 TP2___Thais.Form1.DALMessage("Insertion / Update rejected");
                 return -1;
             }

            /*if (!ds.Tables["Students"].HasErrors)
            {
                return adapter.Update(ds.Tables["Students"]);

            }
            else
            {
                return -1;
            }*/
        }
    }

    internal class Programs
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterProg();
        private static DataSet ds = DataTables.getDataSet();

        internal static DataTable GetPrograms()
        {
            return ds.Tables["Programs"];
        }

        internal static int UpdatePrograms()
        {
            try
            {
                if (!ds.Tables["Programs"].HasErrors)
                {
                    return adapter.Update(ds.Tables["Programs"]);

                }
                return 0;
            }
            catch (Exception)
            {
                TP2___Thais.Form1.DALMessage("Insertion / Update rejected");
                return -1;
            }
            /*if (!ds.Tables["Programs"].HasErrors)
            {
                return adapter.Update(ds.Tables["Programs"]);

            }
            else
            {
                return -1;
            }*/
        }
    }

    internal class Courses
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterCours();
        private static DataSet ds = DataTables.getDataSet();

        private static DataTable displayCours = null;

        internal static DataTable GetCourses()
        {
            return ds.Tables["Courses"];
        }

      

        internal static int UpdateCourses()
        {
            try
            {
                if (!ds.Tables["Courses"].HasErrors)
                {
                    return adapter.Update(ds.Tables["Courses"]);

                }
                return 0;
            }
            catch (Exception)
            {
                TP2___Thais.Form1.DALMessage("Insertion / Update rejected");
                return -1;
            }
           /* if (!ds.Tables["Courses"].HasErrors)
            {
                return adapter.Update(ds.Tables["Courses"]);

            }
            else
            {
                return -1;
            }*/
        }
    }

    internal class Enrollments
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterEnrollment();
        private static DataSet ds = DataTables.getDataSet();

        private static DataTable displayEnrollment = null;

        internal static DataTable GetDisplayEnrollments()
        {
            /* 
             * next line is needed to ensure "delete row"
             * due to the cascade are actually removed.
             */
            ds.Tables["Enrollments"].AcceptChanges();

            var query = (
                   from enrollement in ds.Tables["Enrollments"].AsEnumerable()
                   from student in ds.Tables["Students"].AsEnumerable()
                   from cours in ds.Tables["Courses"].AsEnumerable()
                   from prog in ds.Tables["Programs"].AsEnumerable()
                   where enrollement.Field<string>("StId") == student.Field<string>("StId")
                   where enrollement.Field<string>("CId") == cours.Field<string>("CId")
                   where student.Field<string>("ProgId") == prog.Field<string>("ProgId")
                   select new
                   {
                       StId = student.Field<string>("StId"),
                       StName = student.Field<string>("StName"),
                       CId = cours.Field<string>("CId"),
                       CName = cours.Field<string>("CName"),
                       ProgId = prog.Field<string>("ProgId"),
                       ProgName = prog.Field<string>("ProgName"),
                       FinalGrade = enrollement.Field<Nullable<int>>("FinalGrade")
                       
                   });
            DataTable result = new DataTable();
            result.Columns.Add("StId");
            result.Columns.Add("StName");
            result.Columns.Add("CId");
            result.Columns.Add("CName");
            result.Columns.Add("ProgId");
            result.Columns.Add("ProgName");
            result.Columns.Add("FinalGrade");
            foreach (var x in query)
            {
                object[] allFields = { x.StId, x.StName, x.CId, x.CName, x.ProgId, x.ProgName, x.FinalGrade };
                result.Rows.Add(allFields);
            }
            displayEnrollment = result;
            return displayEnrollment;
        }

        internal static int InsertData(string[] a)
        {
            var test = (
                   from enrollment in ds.Tables["Enrollments"].AsEnumerable()
                   where enrollment.Field<string>("StId") == a[0]
                   where enrollment.Field<string>("CId") == a[1]
                   select enrollment);
            if (test.Count() > 0)
            {
                TP2___Thais.Form1.DALMessage("This assignment already exists");
                return -1;
            }
            if (!TP2___Thais.Form2.sameProgram)
            {
                TP2___Thais.Form1.DALMessage("Please select a course from the same ProgId");
                return -1;
            }
            try
            {
                DataRow line = ds.Tables["Enrollments"].NewRow();
                line.SetField("StId", a[0]);
                line.SetField("CId", a[1]);
                ds.Tables["Enrollments"].Rows.Add(line);

                adapter.Update(ds.Tables["Enrollments"]);

                if (displayEnrollment != null)
                {
                    var query = (
                           from student in ds.Tables["Students"].AsEnumerable()
                           from cours in ds.Tables["Courses"].AsEnumerable()
                           where student.Field<string>("StId") == a[0] 
                           where cours.Field<string>("CId") == a[1]
                           select new
                           {
                               StId = student.Field<string>("StId"),
                               StName = student.Field<string>("StName"),
                               CId = cours.Field<string>("CId"),
                               CName = cours.Field<string>("CName"),
                               FinalGrade = line.Field<Nullable<int>>("FinalGrade")
                           });
                    // Note that Eval =line.Field<Nullable<int>>("Eval") will 
                    // always place null in Eval. It is not needed. 
                    // It is enough to ommit Eval in the select and  
                    // ommit r.Eval in displayAssign.Rows.Add(...)
                    var r = query.Single();
                    displayEnrollment.Rows.Add(new object[] { r.StId, r.StName, r.CId, r.CName, r.FinalGrade });
                }
                return 0;
            }
            catch (Exception)
            {
                TP2___Thais.Form1.DALMessage("Insertion / Update rejected");
                return -1;
            }
        }


        internal static int DeleteData(List<string[]> lId)
        {
            try
            {
                var lines = ds.Tables["Enrollments"].AsEnumerable()
                                .Where(s =>
                                   lId.Any(x => (x[0] == s.Field<string>("StId") && x[1] == s.Field<string>("CId"))));

                foreach (var line in lines)
                {
                    var r = line.Field<Nullable<int>>("FinalGrade");
                    if (r != null)
                    {
                        TP2___Thais.Form1.DALMessage("You can not delete a attributed Final Grade");
                        return -1;
                    }
                    line.Delete();
                }

                adapter.Update(ds.Tables["Enrollments"]);

                if (displayEnrollment != null)
                {
                    foreach (var p in lId)
                    {
                        var r = displayEnrollment.AsEnumerable()
                                .Where(s => (s.Field<string>("StId") == p[0] && s.Field<string>("CId") == p[1]))
                                .Single();
                        displayEnrollment.Rows.Remove(r);
                    }
                }
                return 0;
            }
            catch (Exception)
            {
                TP2___Thais.Form1.DALMessage("Update / Deletion rejected");
                return -1;
            }
        }

        internal static int UpdateFinalGrade(string[] a, Nullable<int> fg)
        {
            try
            {
                var line = ds.Tables["Enrollments"].AsEnumerable()
                                    .Where(s =>
                                      (s.Field<string>("StId") == a[0] && s.Field<string>("CId") == a[1]))
                                    .Single();

                line.SetField("FinalGrade", fg);

                adapter.Update(ds.Tables["Enrollments"]);

                if (displayEnrollment != null)
                {
                    var r = displayEnrollment.AsEnumerable()
                                    .Where(s =>
                                      (s.Field<string>("StId") == a[0] && s.Field<string>("CId") == a[1]))
                                    .Single();
                    r.SetField("FinalGrade", fg);
                }
                return 0;
            }
            catch (Exception)
            {
                TP2___Thais.Form1.DALMessage("Update / Deletion rejected");
                return -1;
            }
        }
    }
}
