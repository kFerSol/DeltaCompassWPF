using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DeltaCompassWPF.Helpers
{
    public static class PasswordHelper
    {
        // Recebe uma variável "SecureString" de qualquer arquivo no projeto.
        public static string HashPassword(SecureString password)
        {
            // Cria uma variável que transforma a senha em string.
            var passwordString = SecureStringToString(password);
            /* Cria uma instância para criptografia sha256 e
               atribui ela para a variável "sha256".*/
            using (var sha256 = SHA256.Create())
            {
                // Calcula o Hash da senha em string.
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(passwordString));
                // retorna o resultado da criptografia.
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public static bool VerifyPassword(SecureString enteredPassword, string storedHash)
        {
            var enteredHash = HashPassword(enteredPassword);
            return enteredHash == storedHash;
        }

        private static string SecureStringToString(SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }
    }
}
