using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class ContractValueChangeMail
    {
        public static void SendContarctValueMail(int ContractID, int id, string ModifiedValue)
        {
            try
            {
                using (var ctx = new CPPDbContext())
                {
                    List<ContractModification> modifiedContract = ctx.ContractModification.Where(c => c.ProgramID == ContractID).ToList();
                    Program contract = ctx.Program.Where(c => c.ProgramID == ContractID).FirstOrDefault();
                    var totalValue = 0.0;
                    for (int i = 1; i < modifiedContract.Count; i++)
                    {
                        if (modifiedContract[i].Id != 0 && modifiedContract[i].Id != null)
                        {
                            totalValue = totalValue + Convert.ToInt32(modifiedContract[i].Value);
                        }
                    }
                    string orignalValue = contract.ContractValue.Replace("$", "").Replace(",", "");
                    totalValue = Convert.ToDouble(orignalValue) + totalValue; 
                    System.Diagnostics.Debug.WriteLine("TotalValue", totalValue);
                    if(contract.ProjectManagerEmail != "")
                    {
                        WebAPI.Services.MailServices.NotifyCostChangeToManager(contract.ProjectManager, contract.ProjectManagerEmail, contract.ContractNumber,
                        contract.ProgramName, ModifiedValue, totalValue.ToString());
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}