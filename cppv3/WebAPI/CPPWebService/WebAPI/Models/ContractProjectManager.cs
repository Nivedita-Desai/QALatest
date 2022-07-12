using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using WebAPI.Helper;
using System.Diagnostics;

namespace WebAPI.Models
{
    [Table("contract_project_manager")]
    public class ContractProjectManager
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProgramId { get; set; }
        public bool IsMailSent { get; set; }

        //Aditya 05052022
        public static void SavePMList(Program program)
        {
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    if (program.PManagerIDS.Count != 0)
                    {
                        List<ContractProjectManager> contractProjectManagers = ctx.ContractProjectManagers.Where(c => c.ProgramId == program.ProgramID).ToList();
                        var pmIds = program.PManagerIDS;
                        List<int> DbUsersID = new List<int>();
                        foreach (var i in contractProjectManagers)
                            DbUsersID.Add(i.UserId);

                        //common id's
                        List<int> commId = pmIds.Intersect(DbUsersID).ToList();
                        //diffrent id's
                        List<int> diffId = pmIds.Except(DbUsersID).ToList();
                        List<int> removeDiffId = DbUsersID.Except(pmIds).ToList();

                        foreach (var k in removeDiffId)
                        {
                            foreach(var e in contractProjectManagers)
                            {
                                if(k == e.UserId)
                                {
                                    ctx.ContractProjectManagers.Remove(e);
                                    ctx.SaveChanges();
                                }
                            }
                        }

                        for (int j = 0; j < diffId.Count; j++)
                        {
                            ContractProjectManager contractProjectManager = new ContractProjectManager();
                            contractProjectManager.ProgramId = program.ProgramID;
                            contractProjectManager.UserId = diffId[j];
                            contractProjectManager.IsMailSent = false;
                            ctx.ContractProjectManagers.Add(contractProjectManager);
                            ctx.SaveChanges();
                        }
                        SendSetUpMail(program.ProgramID);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        // Narayan - Send Mail To Project Manager from Contract
        public static void SendSetUpMail(int ContractID)
        {
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    List<ContractProjectManager> contractProjectManagers = ctx.ContractProjectManagers.Where(c => c.ProgramId == ContractID).ToList();
                    Program contract = ctx.Program.Where(c => c.ProgramID == ContractID).FirstOrDefault();
                    for (int i = 0; i < contractProjectManagers.Count; i++)
                    {
                        if (contractProjectManagers[i].IsMailSent != true)
                        {
                            int userID = contractProjectManagers[i].UserId;
                            User user = ctx.User.Where(u => u.Id == userID).FirstOrDefault();
                            WebAPI.Services.MailServices.ContractProjectManagerMail(user.Email, contract.ContractNumber, contract.ProgramName);
                            contractProjectManagers[i].IsMailSent = true;
                        }
                    }
                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
