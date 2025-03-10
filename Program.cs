namespace StudentDb
{
    internal class Program
    {
        static void Main()
        {
            DbApp app = new DbApp();
            //Use test data
            //app.StudentDbTester();
            // OR
            // Read input file data
             app.ReadStudentDataFromInputFile();

            // Run main DBApp
            app.RunDatabaseApp();
        }
    }
}