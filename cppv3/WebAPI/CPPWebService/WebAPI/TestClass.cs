using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPI.Models;

namespace WebAPI
{
    public class TestClass
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            using (var ctx = new CPPDbContext())
            {
                //Program pgm = new Program() { };

                Program pgm = ctx.Program.First(p => p.ProgramID == 1);
                Console.WriteLine("program : "+pgm.ProgramName);
                //ctx.Program.Add(pgm);
                //ctx.SaveChanges();
            }
 

            Console.WriteLine("Done!");
        }
 
    }
}