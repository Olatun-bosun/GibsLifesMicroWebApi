using System;
using System.Linq;
using System.Collections.Generic;
using Universal.Api.Models;

namespace Universal.Api.Data.Repositories
{
    public partial class Repository
    {
        public string GetNextAutoNumber(string customNo, string numType, string branchID, string subRiskNo = "", string endorseType = "")
        {
            //'$SR$ - Sub Risk Code
            //'$RC$ - Risk Code
            //'$YR$ - Current Year (2 digits)
            //'$MO$ - Current Month (2 Digits)
            //'$00000$ - Serial Number
            //'$BR$ - Branch Code
            //'$BR2$ - Branch Code 2

            //'$P$ - Endorsement something

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
                if (customNo.ToUpper().Contains("AUTO"))
                {
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
                    strAutoNum = strAutoNum.Replace("$YR$", DateTime.Now.ToString("yy"));
                    strAutoNum = strAutoNum.Replace("$MO$", DateTime.Now.ToString("MM"));
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

                    return strAutoNum;
                }
                else
                {
                    return customNo.ToUpper();
                }
            }
            catch 
            {
                return string.Empty;
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
                return string.Empty;
            }
        }
    }
}

//Public Function getNextAutoNumber(ByVal CustomNo As String, ByVal NumType As AutoNumEnum, ByVal BranchID As String, Optional ByVal SubRiskNo As String = "", Optional ByVal EndorseType As String = "") As String
//        '$SR$ - Sub Risk Code
//        '$RC$ - Risk Code
//        '$YR$ - Current Year (2 digits)
//        '$MO$ - Current Month (2 Digits)
//        '$00000$ - Serial Number
//        '$BR$ - Branch Code
//        '$BR2$ - Branch Code 2

//        Try

//            'If SubRiskID = "" Then
//            If CustomNo.ToUpper.Contains("AUTO") Then
//                Dim strAutoNum As String = getSerialNoFormat(NumType.ToString, BranchID).ToString

//                If strAutoNum.IndexOf("$0") >= 0 Then
//                    Dim nextNum As Long = getNextSerialNo(NumType.ToString, BranchID)
//                    If nextNum > 0 Then
//                        Dim len As Integer = strAutoNum.IndexOf("0$") -strAutoNum.IndexOf("$0")
//                        If len< 3 Then len = 3

//                        Dim zeros As String = New String("0", len)
//                        strAutoNum = Replace(strAutoNum, "$" & zeros & "$", Format(nextNum, zeros)) 'formation of number
//                    Else
//                        Throw New Exception("Invalid format in $0000$")
//                    End If
//                Else
//                    Throw New Exception("Missing format $0000$")
//                End If
//                strAutoNum = Replace(strAutoNum, "$SR$", SubRiskNo)
//                strAutoNum = Replace(strAutoNum, "$YR$", Right(Now.Year.ToString, 2))
//                strAutoNum = Replace(strAutoNum, "$MO$", Right("0" & Now.Month.ToString, 2))
//                strAutoNum = Replace(strAutoNum, "$BR$", BranchID)


//                If strAutoNum.Contains("$RC$") Then
//                    strAutoNum = Replace(strAutoNum, "$RC$", (New cls_SubRisks).SelectThis(SubRiskNo).RiskID)
//                End If
//                If strAutoNum.Contains("$BR2$") Then
//                    strAutoNum = Replace(strAutoNum, "$BR2$", (New cls_Branches).SelectThis(BranchID).BranchID2)
//                End If
//                If strAutoNum.Contains("$P$") Then
//                    Dim TypeF As String = ""

//                    Select Case EndorseType
//                        Case "DELETED", "DELETE", "REVERSAL"
//                            TypeF = "C"
//                        Case "RETURN"
//                            TypeF = "C"
//                        Case "RENEWAL", "RENEW"
//                            TypeF = "R"
//                        Case "ADDITIONAL"
//                            TypeF = "E"
//                        Case "NIL", "REDO"
//                            TypeF = "N"
//                        Case Else
//                            TypeF = "A"
//                    End Select


//                    strAutoNum = Replace(strAutoNum, "$P$", TypeF)

//                End If

//                Return strAutoNum
//            Else
//                Return CustomNo.ToUpper
//            End If


//        Catch ex As Exception
//            Return String.Empty
//        End Try
//    End Function

//    Private Function getNextSerialNo(ByVal NumType As String, ByVal BranchID As String, Optional ByVal RiskID As String = "") As Long
//        If NumType = "CLAIM" Then
//            If settings.COMPANY_ABREV = "staco" Then
//                RiskID = BranchID
//            End If

//        End If
//        Try
//            Dim T As List(Of Gibs5.AutoNumber) = (From A In db.AutoNumbers _
//            Where A.NumType = NumType And A.BranchID = "ALL").ToList()

//            If T.Count > 0 Then
//                Try
//                    getNextSerialNo = T(0).NextValue
//                Catch ex As Exception
//                    T(0).NextValue = 1
//                    getNextSerialNo = 1
//                End Try
//                'then increment and update
//                T(0).NextValue += 1
//                db.SubmitChanges()
//            Else 'try specific

//                If RiskID = "" Then
//                    T = (From A In db.AutoNumbers _
//                        Where A.NumType = NumType And A.BranchID = BranchID).ToList()

//                    If T.Count > 0 Then
//                        getNextSerialNo = T(0).NextValue
//                        'then increment and update
//                        T(0).NextValue += 1
//                        db.SubmitChanges()
//                    Else 'no number setup
//                        Return - 1
//                    End If
//                Else
//                    Select Case NumType
//                        Case "CLAIM"
//                            T = (From A In db.AutoNumbers _
//                            Where A.NumType = NumType And A.BranchID = BranchID).ToList()

//                            If T.Count > 0 Then
//                                getNextSerialNo = T(0).NextValue
//                                'then increment and update
//                                T(0).NextValue += 1
//                                db.SubmitChanges()
//                            Else 'no number setup
//                                Return - 1
//                            End If
//                        Case Else
//                            T = (From A In db.AutoNumbers _
//                                Where A.NumType = NumType And A.BranchID = BranchID And A.RiskID = RiskID).ToList()

//                            If T.Count > 0 Then
//                                getNextSerialNo = T(0).NextValue
//                                'then increment and update
//                                T(0).NextValue += 1
//                                db.SubmitChanges()
//                            Else 'no number setup
//                                Return - 1
//                            End If

//                    End Select


//                End If

//            End If

//        Catch ex As Exception
//            Return -2
//        End Try
//    End Function

//    Private Function getSerialNoFormat(ByVal NumType As String, ByVal BranchID As String, Optional ByVal RiskID As String = "") As String
//        If NumType = "CLAIM" Then
//            If settings.COMPANY_ABREV = "staco" Then
//                RiskID = BranchID
//            End If
//        End If
//        Try
//            Dim T As List(Of Gibs5.AutoNumber) = (From A In db.AutoNumbers _
//            Where A.NumType = NumType And A.BranchID = "ALL").ToList()

//            If T.Count > 0 Then
//                Return T(0).Format
//            Else 'try specific

//                If RiskID = "" Then
//                    T = (From A In db.AutoNumbers _
//                        Where A.NumType = NumType And A.BranchID = BranchID).ToList()

//                    If T.Count > 0 Then
//                        Return T(0).Format
//                    Else 'no number setup
//                        Return String.Empty
//                    End If
//                Else

//                    Select Case NumType
//                        Case "CLAIM"
//                            T = (From A In db.AutoNumbers _
//                                Where A.NumType = NumType And A.BranchID = BranchID).ToList()

//                            If T.Count > 0 Then
//                                Return T(0).Format
//                            Else 'no number setup
//                                Return String.Empty
//                            End If

//                        Case Else
//                            T = (From A In db.AutoNumbers _
//                            Where A.NumType = NumType And A.BranchID = BranchID And A.RiskID = RiskID).ToList()

//                            If T.Count > 0 Then
//                                Return T(0).Format
//                            Else 'no number setup
//                                Return String.Empty
//                            End If
//                    End Select
//                End If
//            End If
//        Catch ex As Exception
//            Return String.Empty
//        End Try
//    End Function

