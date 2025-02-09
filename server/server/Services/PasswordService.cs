using System;
using System.Security.Cryptography;
using System.Text;
using Isopoh.Cryptography.Argon2;



namespace server.Services{
public class PasswordService{
   
    private const int MemorySize = 65536; //64mb ram usage
    private const int Iterations = 4;
    private const int Parallelism =4; //CPU Threads


    public static string HashPassword(string password){
       return Argon2.Hash(
        password, 
        timeCost: Iterations, 
        memoryCost: MemorySize, 
        parallelism: Parallelism );

    }

    public static bool VerifyPassword(string password, string storedHash ){
        return Argon2.Verify(storedHash, password);
    }

}
}
