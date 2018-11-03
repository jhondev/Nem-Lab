using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using io.nem1.sdk.Infrastructure.HttpRepositories;
using io.nem1.sdk.Model.Accounts;
using io.nem1.sdk.Model.Blockchain;
using io.nem1.sdk.Model.Mosaics;
using io.nem1.sdk.Model.Transactions;
using io.nem1.sdk.Model.Transactions.Messages;
using System.Reactive.Linq;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace NemDev
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Sending...");
      try
      {
        var result = TransferTest().Result;
        Console.WriteLine("*****Ok!*****");
      }
      catch (System.Exception ex)
      {
        Console.WriteLine("*****NOOOOOO!*****");
        Console.WriteLine(ex.Message);
      }

      Console.ReadLine();
    }

    public static async Task<bool> TransferTest()
    {
      KeyPair keyPair = KeyPair.CreateFromPrivateKey(Config.PrivateKeyMain);

      TransferTransaction transaction = TransferTransaction.Create(
          NetworkType.Types.TEST_NET,
          Deadline.CreateHours(2),
          Address.CreateFromEncoded("TBZQXJ-N7JNQL-N7ZAMC-HFFZAU-WBMPB4-4RMIZU-XKNQ"),
          new List<Mosaic> { Mosaic.CreateFromIdentifier("nem:xem", 1000000) },
          PlainMessage.Create("Sr Alejo")
      );

      SignedTransaction signedTransaction = transaction.SignWith(keyPair);
      Console.WriteLine(signedTransaction.Hash);
      Console.WriteLine(transaction.Fee);
      TransactionResponse response = await
        new TransactionHttp("http://" + Config.Domain + ":7890").Announce(signedTransaction);

      return true;
    }
  }
}
