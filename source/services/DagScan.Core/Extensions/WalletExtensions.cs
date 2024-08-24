using System.Globalization;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using SimpleBase;

namespace DagScan.Core.Extensions;

public static class WalletExtensions
{
    private const string PkcsPrefix = "3056301006072a8648ce3d020106052b8104000a03420004";

    public static string ConvertNodeIdToWalletHash(this string nodeId)
    {
        if (string.IsNullOrEmpty(nodeId))
            throw new ArgumentException("nodeId cannot be null or empty");

        if (nodeId.Length == 128)
        {
            nodeId = $"{PkcsPrefix}{nodeId}";
        }
        else
        {
            throw new ArgumentException("Invalid nodeId length. Must be 128 characters long.");
        }

        var hash = SHA256.HashData(Convert.FromHexString(nodeId));

        var base58Encoded = Base58.Bitcoin.Encode(hash);
        var finalNodeId = base58Encoded[^36..];

        var checkDigits = Regex.Replace(finalNodeId, "[^0-9]+", "");
        var checkDigit = checkDigits
            .Select(c => int.Parse(c.ToString(), CultureInfo.InvariantCulture))
            .Sum();
        checkDigit = checkDigit > 8 ? checkDigit % 9 : checkDigit;

        var dagAddress = $"DAG{checkDigit}{finalNodeId}";
        return dagAddress;
    }
}
