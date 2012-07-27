using System;
using System.Collections.Generic;

using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;

public class Account
{
    public int ID { get; set; }
    public double Balance { get; set; }
}

public class Program
{
    static void Main(string[] args)
    {
        var checkAccounts = new List<Account> {
                   new Account {
                                      ID = 345,
                                      Balance = 541.27
                               },
                   new Account {
                                      ID = 123,
                                      Balance = -127.44
                               }
               };

        DisplayInExcel(checkAccounts, (account, cell) =>
        {
            //Multiline lambda
            cell.Value2 = account.ID;
            cell.get_Offset(0, 1).Value2 = account.Balance;

            if (account.Balance < 0)
            {
                cell.Interior.Color = 255;
                cell.get_Offset(0, 1).Interior.Color = 255;
            }
        });

        var word = new Word.Application();
        word.Visible = true;
        word.Documents.Add();
        word.Selection.PasteSpecial(Link: true, DisplayAsIcon: true);
    }

    public static void DisplayInExcel(IEnumerable<Account> accounts,
                           Action<Account, Excel.Range> DisplayFunc)
    {
        var xl = new Excel.Application();

        xl.Workbooks.Add();
        xl.Visible = true;
        xl.Cells[1, 1] = "ID";
        xl.Cells[1, 2] = " Balance";
        xl.Cells[2, 1].Select();
        foreach (var ac in accounts)
        {
            DisplayFunc(ac, xl.ActiveCell);
            xl.ActiveCell.get_Offset(1, 0).Select();
        }

        xl.get_Range("A1:B3").Copy();

        xl.Columns[1].AutoFit();
        xl.Columns[2].AutoFit();
    }
}