# Login module - how to add it

1. Copy the contents of this folder into your project folder
   (the one that contains FuturePathStudentAdvisingSystem.csproj):
     frmLogin.cs, frmLogin.Designer.cs   -> project root, next to Form1.cs
     Security/                           -> new folder
     DataAccess/UserAccountDAL.cs        -> into your existing DataAccess folder
   SDK-style projects pick up new .cs files automatically. No "Add Existing Item" needed.
   (If Visual Studio doesn't show them, click "Show All Files" in Solution Explorer.)

2. In Program.cs change
       Application.Run(new Form1());
   to
       Application.Run(new frmLogin());

3. Run it. Guest and Learner go to your existing profile screen (Form1).
