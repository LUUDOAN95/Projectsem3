using System.Security.Cryptography;
using System.Text;

namespace eproject3.Models
{
    public class VNPayConfig
    {
        public static string ConfigName = "Sandbox";
        public static string Version = "2.1.0";
        public static string Command = "pay";
        public static string TmnCode = "DEMOV210"; // Replace with your TmnCode from VNPAY
        public static string HashSecret = "RAOEXHYVSDDIIENYWSLDIIZTANXUXZFJ"; // Replace with your HashSecret from VNPAY
        public static string PaymentUrl = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
        public static string ReturnUrl = "http://localhost:5258/Payment/VNPayReturn";
        public static string OrderType = "other";
        public static string CurrCode = "VND";
        public static string Locale = "vn";

        public static string GetIpAddress()
        {
            string ipAddress;
            try
            {
                ipAddress = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
                    .Select(p => p.GetIPProperties())
                    .SelectMany(p => p.UnicastAddresses)
                    .First(p => p.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork 
                            && !System.Net.IPAddress.IsLoopback(p.Address))
                    .Address.ToString();
            }
            catch (Exception)
            {
                ipAddress = "127.0.0.1";
            }
            return ipAddress;
        }

        public static string HmacSHA512(string key, string inputData)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }
            return hash.ToString();
        }
    }
} 