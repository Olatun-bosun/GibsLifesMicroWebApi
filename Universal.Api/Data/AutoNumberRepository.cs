using System;
using System.Linq;
using System.Collections.Generic;
using Universal.Api.Models;

namespace Universal.Api.Data.Repositories
{
    public partial class Repository
    {
        public string GetNextAutoNumber(string numType, string branchID, string subRiskNo = "", string endorseType = "", DateTime optionalDate = default)
        {
            //'$SR$ - Sub Risk Code
            //'$RC$ - Risk Code
            //'$YR$ - Current Year (2 digits)
            //'$MO$ - Current Month (2 Digits)
            //'$00000$ - Serial Number
            //'$BR$ - Branch Code
            //'$BR2$ - Branch Code 2

            //'$P$ - Endorsement something

            if (optionalDate == default)
                optionalDate = DateTime.Today;

            static string GetEndorsementCode(string endorseType)
            {
                switch (endorseType)
                {
                    case "DELETE":
                    case "DELETED":
                    case "REVERSAL":
                    case "RETURN":
                        return "C";

                    case "RENEW":
                    case "RENEWAL":
                        return "R";

                    case "ADDITIONAL":
                        return "E";

                    case "NIL":
                    case "REDO":
                        return "N";

                    default:
                        return "A";
                }
            }

            try
            {
                //if (customNo.ToUpper().Contains("AUTO"))
                //{
                    string strAutoNum = GetSerialNoFormat(numType, branchID);

                    if (strAutoNum.IndexOf("$0") >= 0)
                    {
                        long nextNum = GetNextSerialNo(numType, branchID);

                        if (nextNum > 0)
                        {
                            int len = strAutoNum.IndexOf("0$") - strAutoNum.IndexOf("$0");
                            if (len < 3) len = 3;

                            string zeros = new string('0', len);
                            strAutoNum = strAutoNum.Replace($"${zeros}$", nextNum.ToString(zeros)); //formation of number
                        }
                        else
                        {
                            throw new Exception("Invalid format in $0000$");
                        }
                    }
                    else
                    {
                        throw new Exception("Missing format $0000$");
                    }
                    strAutoNum = strAutoNum.Replace("$SR$", subRiskNo);
                    strAutoNum = strAutoNum.Replace("$YR$", optionalDate.ToString("yy"));
                    strAutoNum = strAutoNum.Replace("$MO$", optionalDate.ToString("MM"));
                    strAutoNum = strAutoNum.Replace("$BR$", branchID);


                    if (strAutoNum.Contains("$RC$"))
                    {
                        var risk = _db.SubRisks.Where(x => x.SubRiskID == subRiskNo).First();
                        strAutoNum = strAutoNum.Replace("$RC$", risk.RiskID);
                    }
                    if (strAutoNum.Contains("$BR2$"))
                    {
                        var branch = _db.Branches.Where(x => x.BranchID == branchID).First();
                        strAutoNum = strAutoNum.Replace("$BR2$", branch.BranchID2);
                    }

                    if (strAutoNum.Contains("$P$"))
                    {
                        strAutoNum = strAutoNum.Replace("$P$", GetEndorsementCode(endorseType));
                    }

                    return strAutoNum.ToUpper();
                //}
                //else
                //{
                //    return customNo.ToUpper();
                //}
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error in GetNextAutoNumber()\n\r" + ex.ToString());
            }
        }

        private long GetNextSerialNo(string numType, string branchID, string riskID = "")
        {
            static long ResultOf(List<AutoNumber> list, long errorValue)
            {
                if (list.Count() > 0)
                {
                    long result = 0;
                    if (list[0].NextValue.HasValue)
                        result = list[0].NextValue.Value;
                    else
                        result = 1;

                    //'then increment and update
                    list[0].NextValue += 1;
                    //_db.SaveChanges();
                    return result;
                }
                else
                    return errorValue;
            }

            try
            {
                var query = _db.AutoNumbers.Where(x => x.NumType == numType);
                var T = query.Where(x => x.BranchID == "ALL").ToList();

                if (T.Count() > 0)
                    return ResultOf(T, 1);
                else
                {
                    query = query.Where(x => x.BranchID == branchID);

                    if (riskID == "")
                        return ResultOf(query.ToList(), -1);
                    else
                    {
                        if (numType == "CLAIM")
                            return ResultOf(query.ToList(), -1);
                        else
                            return ResultOf(query.Where(x => x.RiskID == riskID).ToList(), -1);
                    }
                }
            }
            catch 
            {
                return -2;
            }
        }

        private string GetSerialNoFormat(string numType, string branchID, string riskID = "")
        {
            static string ResultOf(List<AutoNumber> list)
            {
                if (list.Count() > 0)
                    return list[0].Format;
                else
                    //'no number setup
                    return string.Empty;
            }

            try
            {
                var query = _db.AutoNumbers.Where(x => x.NumType == numType);
                var T = query.Where(x => x.BranchID == "ALL").ToList();

                if (T.Count() > 0)
                    return T[0].Format;
                else
                {
                    query = query.Where(x => x.BranchID == branchID);

                    if (riskID == "")
                        return ResultOf(query.ToList());
                    else
                    {
                        if (numType == "CLAIM")
                            return ResultOf(query.ToList());
                        else
                            return ResultOf(query.Where(x => x.RiskID == riskID).ToList());
                    }
                }
            }
            catch 
            {
                throw new InvalidOperationException("Error in GetSerialNoFormat()");
            }
        }
    }
}
