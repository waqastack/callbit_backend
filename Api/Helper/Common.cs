

using System;
using System.Text;
using System.Security.Cryptography;

namespace Api.Helper
{
  public static class Common
  {
    private static string encryptionParaphrase = "test";
    private static string _EncryptionPrefix = "aslkfn veiurbgfajsdnfa.m";
    private static string _EncryptionPostfix = "aslkfnuefjdb la sdfa.m";
    public static string Encrypt(string val)
    {
      return Convert.ToBase64String(Encoding.Unicode.GetBytes($"{_EncryptionPrefix}{val}{_EncryptionPostfix}"));
    }
    public static string Decrypt(string val)
    {
      var str = Encoding.Unicode.GetString(Convert.FromBase64String(val));
      str = str.Replace(_EncryptionPrefix,"");
      str = str.Replace(_EncryptionPostfix, "");
      return str;
    }
  }
}
