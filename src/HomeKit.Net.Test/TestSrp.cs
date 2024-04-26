using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using HomeKit.Net;
using HomeKit.Net.Enums;
using NSec.Cryptography;
using Utils= HomeKit.Net.Utils;
namespace HomeKit.NetTest;

public class TestSrp
{
    [Fact]
    public void TestSrpGN()
    {
        var result =
            "5809605995369958062791915965639201402176612226902900533702900882779736177890990861472094774477339581147373410185646378328043729800750470098210924487866935059164371588168047540943981644516632755067501626434556398193186628990071248660819361205119793693985433297036118232914410171876807536457391277857011849897410207519105333355801121109356897459426271845471397952675959440793493071628394122780510124618488232602464649876850458861245784240929258426287699705312584509625419513463605155428017165714465363094021609290561084025893662561222573202082865797821865270991145082200656978177192827024538990239969175546190770645685893438011714430426409338676314743571154537142031573004276428701433036381801705308659830751190352946025482059931306571004727362479688415574702596946457770284148435989129632853918392117997472632693078113129886487399347796982772784615865232621289656944284216824611318709764535152507354116344703769998514148343807";
        var bytes = Constants.SrpNStr.ToHexByte();
        var n = new BigInteger(bytes, true, true);
        var g= new BigInteger(int.Parse(Constants.SrpGStr));
        Assert.Equal(result,n.ToString());
        Assert.Equal("5",g.ToString());
    }
    
    [Fact]
    public void TestSrpResult()
    {
        var identityBytes = Encoding.UTF8.GetBytes("Pair-Setup");
        var pinCode = Encoding.UTF8.GetBytes("670-00-613");
        var salt = new byte[] {196, 151, 109, 27, 100, 245, 45, 64, 79, 89, 97, 46, 126, 217, 173, 34};
        var b = System.Numerics.BigInteger.Parse("39024249094872699906964274658921292302198529737823718295914983273159161725876");
        var srpServer= new SrpServer(SHA512.Create().ComputeHash,identityBytes,pinCode,salt,b:b);
        srpServer.SetA(new byte[]
        {
            1, 246, 119, 18, 111, 118, 3, 94, 68, 131, 136, 130, 248, 149, 32, 12, 164, 254, 3, 238, 22, 51, 152, 74,
            235, 165, 190, 237, 103, 40, 18, 244, 248, 77, 205, 105, 34, 180, 72, 24, 2, 194, 163, 209, 125, 7, 174, 6,
            82, 96, 177, 184, 55, 174, 9, 36, 21, 113, 167, 112, 236, 255, 93, 22, 213, 118, 241, 159, 47, 163, 203,
            132, 29, 163, 203, 141, 3, 206, 124, 103, 121, 37, 157, 205, 119, 111, 208, 64, 38, 41, 58, 173, 105, 208,
            3, 142, 5, 155, 220, 77, 117, 80, 201, 39, 102, 26, 235, 175, 117, 157, 113, 38, 108, 35, 167, 45, 241, 55,
            165, 155, 156, 216, 239, 160, 255, 217, 74, 213, 216, 132, 167, 181, 237, 129, 70, 215, 94, 170, 182, 124,
            135, 10, 88, 74, 196, 186, 68, 67, 154, 10, 175, 244, 30, 120, 157, 104, 121, 167, 148, 215, 230, 124, 145,
            52, 90, 54, 97, 177, 0, 210, 94, 90, 130, 32, 2, 156, 64, 216, 243, 85, 190, 73, 116, 138, 198, 230, 194,
            194, 183, 49, 113, 18, 123, 169, 183, 43, 166, 151, 219, 230, 252, 70, 173, 37, 157, 158, 196, 238, 1, 153,
            188, 7, 227, 246, 123, 123, 48, 217, 205, 56, 3, 103, 168, 5, 238, 245, 168, 175, 238, 254, 61, 218, 19,
            178, 127, 205, 50, 90, 49, 132, 236, 93, 84, 219, 12, 200, 30, 14, 183, 21, 116, 88, 186, 179, 165, 97, 106,
            189, 197, 3, 139, 216, 141, 110, 199, 14, 10, 131, 185, 253, 228, 80, 176, 83, 78, 206, 65, 177, 244, 55,
            210, 183, 7, 10, 63, 212, 165, 77, 196, 2, 184, 73, 199, 134, 226, 41, 5, 229, 139, 126, 223, 88, 181, 78,
            51, 83, 179, 237, 32, 110, 33, 122, 77, 8, 148, 180, 169, 3, 193, 60, 88, 205, 206, 162, 64, 176, 225, 170,
            150, 32, 29, 51, 127, 97, 169, 202, 5, 130, 192, 49, 201, 146, 177, 103, 16, 161, 108, 185, 95, 123, 232,
            242, 169, 130, 216, 6, 82, 82, 165, 244, 163, 156, 162, 147, 234, 63, 184, 197, 111, 101, 199, 59, 193, 248,
            28, 20, 213, 45, 144, 230, 2, 65, 182, 85
        });
        
        Assert.True(srpServer.B==BigInteger.Parse("5675319999338081874455704622358011185026278182478375756585527678083533631450453936276409496406377459655197450071445883300938212196565246530065043426760069821770979242423058650456742676633081901784709922224925715529855582626375103824988088038971142519642422751460927067134108575906622273951307716272726661763522799568028924958940039158540393381178152860070422851647550193549839146102364305922186483553310612163367925447293609906744916295573195913789141013833331739752966994723419259233482571470312665911870471886401007904061470521386054399106757676654696722657213945906911162350566300011075940399127724656532694612849278339784038954984661727890855670310299590956117157331242302030888167848359456184894780629311643883843219172893686028985363953802077371921603482176948187489217182173854592505084657397093441719054995432852311719350510949377493161844243010721048496153707148096758585710602476039072718031956843878508217091465054"));

        Assert.True(srpServer.K==BigInteger.Parse("7053486700927150616815426770676129831154148085404494109811510118121025368385363216037964841435446932662343678508677235488008746472607938197007361700361778"));
        
        Assert.True(srpServer.TestCompareString("'66,207,175,21,237,213,186,177,58,127,162,69,179,207,53,187,57,26,25,232,80,93,171,185,208,88,11,162,79,245,77,250,7,39,52,21,99,204,75,74,110,1,153,184,100,131,211,177,113,208,123,132,80,225,201,241,129,246,61,194,210,244,161,205'",srpServer.HAMK));
        
        Assert.True(srpServer.TestCompareString("'189,212,236,80,68,77,56,106,165,250,81,215,1,63,115,246,197,27,63,110,157,86,108,210,232,112,207,49,51,216,167,198,130,93,231,16,133,81,146,116,106,186,167,37,123,176,68,123,15,110,251,16,107,88,173,176,164,149,241,21,26,149,213,207'",srpServer.M));

        Assert.True(srpServer.S==BigInteger.Parse("2208876457805064203299598719518489345105450766783866739778401066020442796932018636491736109858943406410033381469647743162842631459609810535479473283490025581309921912215558664562093778140619597178024492949892181266083330574876832975811517969662907912541205095910950342852581405460231090015205665820291092548357508160790927998036605608572835101077282195346392522968345552907162038747591442512109563357182928185064836482292254165130759823278213074009533732971913335899485110312378181748377929649193934923892571559965072703301297265597075855281041467896891954587490425016651691464743676135538499578141808071339719266109119179610176831853889288869473142800551446910820392581634905164138920596648962991120257210280946373892209717477236565633834802887967840183599620750649906243487570599486390119352288807753547097679213989890673782435330524015342378236307101144526149857157310086474690468297806071129298655060312612496706861762955"));
        Assert.True(srpServer.k==BigInteger.Parse("8891118944006259431156568541843809053371474718154946070525699599564743247786811275097952247025117806925219847643897478119979876683245412022290811230509536"));
        Assert.True(srpServer.u==BigInteger.Parse("552635817670013531755431980382637771771523672555065088467943528366110549719254486438104184473904451742297067166272415062177414472382577816807210260108018"));
        Assert.True(srpServer.v==BigInteger.Parse("1920511244547200584657980660741189713420669883494809818167848110230880605870355404892206448112291964537727959140612899603313905157920839242986058168776764650047732631474826694611662021334845085118418427729926275130106407936354089240908061782864378492944994503301813887759099552112392742355099688315577156823119972616526612925614258446308060962470430771462918200161168380285860555641532591887096494431055853662030189029525801680865897712509424712540256507494999505292387150570295188971778690290065069655904540924013667740447927667707714394448982043120760322643160642904211519697965803724649790548803758338586224104995216600409914281182168652062626388766743421451817502355076557559285102520155796175218783467225164969112776000708233134309843235336623634105532443311920836374774285957613612970947594010400032191415870252632532397784105089560214937643506817701613839502099522274710249958067186800439736858602962450254764056252058"));

    }
    
     [Fact]
    public void TestSrpResult2()
    {
        var identityBytes = Encoding.UTF8.GetBytes("Pair-Setup");
        var pinCode = Encoding.UTF8.GetBytes("008-19-772");
        var salt = new byte[] {184,152,72,4,182,5,151,27,74,75,205,87,68,81,86,133};
        var b = System.Numerics.BigInteger.Parse("58596985450258352368330490139345916976662962435594580343715530837701013423432");
        var srpServer= new SrpServer(SHA512.Create().ComputeHash,identityBytes,pinCode,salt,b:b);
        srpServer.SetA(new byte[]
        {
            247,163,115,101,155,49,161,56,165,9,21,17,58,115,149,166,54,11,242,108,202,87,37,80,87,137,146,61,93,4,214,4,57,34,7,255,236,248,43,100,74,138,237,212,229,153,65,244,105,138,239,233,254,30,207,104,47,117,229,39,135,28,37,29,101,60,64,44,173,86,156,33,18,76,102,32,177,86,187,0,69,104,177,210,89,112,91,209,154,219,164,75,87,168,250,161,120,52,53,141,55,242,25,178,151,132,183,88,79,166,109,129,105,215,133,171,75,60,64,51,55,146,149,1,184,180,29,44,123,167,129,234,247,17,15,50,169,224,104,27,146,42,172,251,122,71,53,95,62,122,200,216,176,45,123,47,152,246,172,88,245,84,235,142,61,21,194,247,19,18,77,30,116,129,91,212,116,66,194,9,143,235,112,135,14,217,55,46,185,105,2,185,141,82,67,66,219,209,85,255,217,210,117,11,226,42,245,36,36,79,51,125,64,180,201,131,219,101,1,88,159,110,23,8,25,165,13,114,229,102,116,126,87,43,142,50,126,195,209,106,77,62,226,124,240,136,112,59,98,29,122,30,215,101,5,150,134,96,225,11,1,221,85,66,168,254,75,177,50,208,21,92,86,51,70,131,91,19,220,29,242,153,241,199,191,33,94,188,183,154,189,156,224,213,159,101,228,219,212,62,130,212,129,217,203,137,162,194,93,78,232,233,251,137,247,251,10,168,100,187,140,159,27,228,99,135,80,187,55,40,134,75,45,184,206,129,9,4,141,125,70,83,80,238,32,158,42,245,54,20,123,147,57,135,171,113,144,74,71,96,174,66,39,141,105,1,152,32,159,146,114,85,214,11,23,135,178,216,101,234,8,57,36,128
        });
        
        Assert.True(srpServer.B==BigInteger.Parse("5574835763182594775412635639190853226609948267642937613957510604981179369053851847120489668911592243671014921651806218398111437255852000095110834038865363871251279266813225696746040269361919946089671458570425960588139611655798773427274757016245053900209600072871246884118313832780330988249954962654086016045211358659647062807918503706601109485525213281949300709512885173394404859051343471370452674131766166115238362886809378724335558523202632940110674431979146585004425390012111110380781464711246550849307046957396579517163345164168376186419022827520473872216017071901039702781695774882927379639075964032038109535994678234584431192385682145370973142800869258515459859533171340055127218585023720229416998907620702492546912634765789106639075915833685519263305168115632071978312146116364339996817002422632792187183750752896331089453030842283927507028021918001513272966575948325447716591770767617097307256537165107985525588199609"));

        Assert.True(srpServer.K==BigInteger.Parse("13250243720859205533980824668671924858364837102363154585928685650240228015414633529149928749875138823096044771366309257781841674259230346346322291787090153"));
        
        Assert.True(srpServer.TestCompareString("'109,48,173,29,105,179,170,89,146,60,130,40,142,218,61,17,92,108,205,135,134,255,164,29,84,45,112,2,222,59,235,245,41,238,124,255,247,93,93,165,18,145,171,31,243,173,114,47,51,33,77,57,252,251,14,43,226,236,127,135,78,91,53,212'",srpServer.HAMK));
        //
        Assert.True(srpServer.TestCompareString("'67,58,203,207,191,29,191,227,79,172,249,72,210,104,69,25,211,101,176,102,160,155,176,43,91,209,101,114,63,5,133,2,137,25,190,155,180,227,225,203,163,165,115,38,214,75,169,79,250,160,235,175,156,172,154,125,183,254,92,119,171,54,170,52'",srpServer.M));
        var hc= srpServer.Verify(new byte[]
        {
            67, 58, 203, 207, 191, 29, 191, 227, 79, 172, 249, 72, 210, 104, 69, 25, 211, 101, 176, 102, 160, 155, 176,
            43, 91, 209, 101, 114, 63, 5, 133, 2, 137, 25, 190, 155, 180, 227, 225, 203, 163, 165, 115, 38, 214, 75,
            169, 79, 250, 160, 235, 175, 156, 172, 154, 125, 183, 254, 92, 119, 171, 54, 170, 52
        });
        //
        // Assert.True(srpServer.S==BigInteger.Parse("2208876457805064203299598719518489345105450766783866739778401066020442796932018636491736109858943406410033381469647743162842631459609810535479473283490025581309921912215558664562093778140619597178024492949892181266083330574876832975811517969662907912541205095910950342852581405460231090015205665820291092548357508160790927998036605608572835101077282195346392522968345552907162038747591442512109563357182928185064836482292254165130759823278213074009533732971913335899485110312378181748377929649193934923892571559965072703301297265597075855281041467896891954587490425016651691464743676135538499578141808071339719266109119179610176831853889288869473142800551446910820392581634905164138920596648962991120257210280946373892209717477236565633834802887967840183599620750649906243487570599486390119352288807753547097679213989890673782435330524015342378236307101144526149857157310086474690468297806071129298655060312612496706861762955"));
        // Assert.True(srpServer.k==BigInteger.Parse("8891118944006259431156568541843809053371474718154946070525699599564743247786811275097952247025117806925219847643897478119979876683245412022290811230509536"));
        // Assert.True(srpServer.u==BigInteger.Parse("552635817670013531755431980382637771771523672555065088467943528366110549719254486438104184473904451742297067166272415062177414472382577816807210260108018"));
        // Assert.True(srpServer.v==BigInteger.Parse("1920511244547200584657980660741189713420669883494809818167848110230880605870355404892206448112291964537727959140612899603313905157920839242986058168776764650047732631474826694611662021334845085118418427729926275130106407936354089240908061782864378492944994503301813887759099552112392742355099688315577156823119972616526612925614258446308060962470430771462918200161168380285860555641532591887096494431055853662030189029525801680865897712509424712540256507494999505292387150570295188971778690290065069655904540924013667740447927667707714394448982043120760322643160642904211519697965803724649790548803758338586224104995216600409914281182168652062626388766743421451817502355076557559285102520155796175218783467225164969112776000708233134309843235336623634105532443311920836374774285957613612970947594010400032191415870252632532397784105089560214937643506817701613839502099522274710249958067186800439736858602962450254764056252058"));

    }
    
    [Fact]
    public void TestBytesToBigInteger()
    {
        var result = "40808098080904267132172453301642759086045942683221372294813391353642838190224";
        
        var temp= new BigInteger(new byte[]
        {
            90, 56, 144, 163, 254, 214, 63, 40, 145, 109, 105, 237, 55, 32, 88, 250, 105, 109, 113, 172, 232, 174, 85,
            17, 212, 199, 165, 79, 207, 64, 232, 144
        },true,true);
        Assert.Equal(result,temp.ToString());
    }
    
    [Fact]
    public void TestPairing3To5()
    {
        var PAIRING_3_SALT = "Pair-Setup-Encrypt-Salt".ToBytes();
        var PAIRING_3_INFO = "Pair-Setup-Encrypt-Info".ToBytes();
        var s = BigInteger.Parse(
            "860380235096424965640732240978482803298372130612015281305864284678277487813497180652772356049401754928679445595055861953127679106349359171231330071067251");

        var sb= s.ToByteArray(true, true);
        var bstr =
            "'16,109,115,115,219,204,214,117,4,231,181,229,133,28,174,232,169,253,196,209,51,215,28,9,52,152,53,116,172,229,131,254,249,74,204,137,252,172,180,232,170,46,88,202,61,26,83,128,175,67,199,58,197,184,136,211,201,132,248,92,202,169,186,115'"
                .GetSpecialStrToBytes();
        var f = sb.CompareTwoBytes(bstr);

        var hkdfTarget =
            "'124,200,230,252,107,87,120,60,22,215,211,119,35,64,243,116,239,89,109,22,160,22,98,68,105,232,27,140,65,145,52,228'".GetSpecialStrToBytes();

        var hkdf = HKDF.DeriveKey(new HashAlgorithmName(nameof(SHA512)), bstr, 32, PAIRING_3_SALT, PAIRING_3_INFO);
        
        Assert.True(hkdfTarget.CompareTwoBytes(hkdf));

        var padTlsNonce = Utils.PadTlsNonce("PS-Msg05".ToBytes());
        var padTlsNonceTarget = "'0,0,0,0,80,83,45,77,115,103,48,53'".GetSpecialStrToBytes();
        Assert.True(padTlsNonce.CompareTwoBytes(padTlsNonceTarget));

        var encryptedData = "'233,160,25,61,214,114,32,180,120,191,227,27,93,254,102,74,19,238,178,118,214,46,7,248,118,228,23,94,152,145,248,57,189,104,62,187,3,190,11,181,165,59,243,119,175,126,168,31,231,37,105,176,30,223,237,141,132,88,105,52,59,226,4,241,253,208,239,149,195,253,69,230,215,172,231,45,231,250,117,249,167,145,68,224,34,244,23,208,96,2,167,38,29,239,172,68,4,211,237,130,216,21,36,237,214,101,153,169,105,8,16,38,127,114,74,86,132,75,46,169,25,177,111,192,86,151,245,36,21,77,178,245,200,152,235,55,40,181,120,98,108,223,24,35,201,100,166,146,70,225,119,146,200,126'".GetSpecialStrToBytes();
        
        using var k = Key.Import(AeadAlgorithm.ChaCha20Poly1305, hkdf, KeyBlobFormat.RawSymmetricKey);
        var decryptedData = AeadAlgorithm.ChaCha20Poly1305.Decrypt(k, padTlsNonce, new byte[0], encryptedData);

        var decryptedDataTarget =
            "'1,36,69,69,48,48,53,54,70,53,45,49,49,66,70,45,52,50,51,54,45,57,54,49,50,45,50,50,66,54,54,56,53,49,70,66,48,66,3,32,203,191,202,190,130,36,28,52,50,26,157,80,24,70,93,239,151,221,213,194,111,119,254,106,191,83,243,50,101,33,38,33,10,64,218,74,72,155,145,56,222,24,169,108,117,38,212,112,238,167,130,54,232,231,162,135,30,110,11,49,69,45,235,133,215,229,155,172,234,51,210,116,140,195,100,22,251,19,249,246,193,115,135,51,238,245,119,2,48,240,119,61,121,21,218,230,16,12'"
                .GetSpecialStrToBytes();

        Assert.True(decryptedDataTarget.CompareTwoBytes(decryptedData));
        var tlvItems= new Tlv().Decode(decryptedData);
        var clientUserName=tlvItems.FirstOrDefault(it => it.Tag[0]==(byte)Hap_Tlv_Tags.USERNAME);
        var clientLtpk=tlvItems.FirstOrDefault(it => it.Tag[0]==(byte)Hap_Tlv_Tags.PUBLIC_KEY);
        var clientProof=tlvItems.FirstOrDefault(it => it.Tag[0]==(byte)Hap_Tlv_Tags.PROOF);

        var clientUserNameTarget = "'69,69,48,48,53,54,70,53,45,49,49,66,70,45,52,50,51,54,45,57,54,49,50,45,50,50,66,54,54,56,53,49,70,66,48,66'".GetSpecialStrToBytes();
        Assert.True(clientUserName.Value.CompareTwoBytes(clientUserNameTarget));
        
        var clientLtpkTarget = "'203,191,202,190,130,36,28,52,50,26,157,80,24,70,93,239,151,221,213,194,111,119,254,106,191,83,243,50,101,33,38,33'".GetSpecialStrToBytes();
        Assert.True(clientLtpk.Value.CompareTwoBytes(clientLtpkTarget));
        
        var clientProofTarget = "'218,74,72,155,145,56,222,24,169,108,117,38,212,112,238,167,130,54,232,231,162,135,30,110,11,49,69,45,235,133,215,229,155,172,234,51,210,116,140,195,100,22,251,19,249,246,193,115,135,51,238,245,119,2,48,240,119,61,121,21,218,230,16,12'".GetSpecialStrToBytes();
        Assert.True(clientProof.Value.CompareTwoBytes(clientProofTarget));
        
        var PAIRING_4_SALT = "Pair-Setup-Controller-Sign-Salt".ToBytes();
        var PAIRING_4_INFO = "Pair-Setup-Controller-Sign-Info".ToBytes();
        
        var outputKey = HKDF.DeriveKey(new HashAlgorithmName(nameof(SHA512)), bstr, 32, PAIRING_4_SALT, PAIRING_4_INFO);
        var outputKeyTarget = "'140,226,55,142,69,119,178,178,248,109,52,103,228,62,115,65,23,219,189,174,229,50,233,228,202,248,62,119,148,30,225,212'".GetSpecialStrToBytes();
        Assert.True(outputKey.CompareTwoBytes(outputKeyTarget));

        var data = Utils.MergeBytes(outputKey, clientUserNameTarget, clientLtpkTarget);
        var dataTarget = "'140,226,55,142,69,119,178,178,248,109,52,103,228,62,115,65,23,219,189,174,229,50,233,228,202,248,62,119,148,30,225,212,69,69,48,48,53,54,70,53,45,49,49,66,70,45,52,50,51,54,45,57,54,49,50,45,50,50,66,54,54,56,53,49,70,66,48,66,203,191,202,190,130,36,28,52,50,26,157,80,24,70,93,239,151,221,213,194,111,119,254,106,191,83,243,50,101,33,38,33'".GetSpecialStrToBytes();
        Assert.True(dataTarget.CompareTwoBytes(data));
        
        var a = SignatureAlgorithm.Ed25519;

        var ed25519Key = PublicKey.Import(a, clientLtpkTarget, KeyBlobFormat.RawPublicKey);
        var ed25519VerifyResult= Ed25519.Ed25519.Verify(ed25519Key, data, clientProofTarget);
        Assert.True(ed25519VerifyResult);

        var PAIRING_5_SALT = "Pair-Setup-Accessory-Sign-Salt".ToBytes();
        var PAIRING_5_INFO = "Pair-Setup-Accessory-Sign-Info".ToBytes();
        
        var outputKey5=HKDF.DeriveKey(new HashAlgorithmName(nameof(SHA512)), bstr, 32, PAIRING_5_SALT, PAIRING_5_INFO);
        var outputKey5Target = "'213,193,132,161,72,214,233,25,8,188,199,126,180,33,231,81,204,65,148,248,144,213,238,46,116,113,175,57,234,61,249,172'".GetSpecialStrToBytes();
        Assert.True(outputKey5.CompareTwoBytes(outputKey5Target));

        var serverPublic = "'104,221,166,94,91,70,48,214,207,179,29,135,10,237,70,202,128,41,210,250,224,173,229,160,185,241,46,11,27,196,92,1'".GetSpecialStrToBytes();
        var mac = "'48,56,58,56,54,58,57,51,58,52,65,58,69,70,58,54,49'".GetSpecialStrToBytes();

        var materialTarget = "'213,193,132,161,72,214,233,25,8,188,199,126,180,33,231,81,204,65,148,248,144,213,238,46,116,113,175,57,234,61,249,172,48,56,58,56,54,58,57,51,58,52,65,58,69,70,58,54,49,104,221,166,94,91,70,48,214,207,179,29,135,10,237,70,202,128,41,210,250,224,173,229,160,185,241,46,11,27,196,92,1'".GetSpecialStrToBytes();
        var material = Utils.MergeBytes(outputKey5, mac, serverPublic);
        Assert.True(material.CompareTwoBytes(materialTarget));

        var primaryKey5 =
            "'108,209,81,76,226,223,26,73,241,95,148,28,21,78,87,44,196,162,220,1,25,220,137,227,223,5,252,52,13,246,193,52'"
                .GetSpecialStrToBytes();
        var ed25519Key5 = Key.Import(a, primaryKey5, KeyBlobFormat.RawPrivateKey);
        var serverProof5= Ed25519.Ed25519.Sign(ed25519Key5, material);
        var serverProof5Target = "'4,146,164,224,76,122,165,93,190,143,212,236,31,242,163,173,103,116,78,185,55,237,85,205,71,3,98,197,112,57,252,96,72,173,220,122,21,245,201,85,178,27,76,98,136,55,47,96,14,239,184,10,40,16,221,15,178,178,5,169,126,127,203,12'".GetSpecialStrToBytes();
        Assert.True(serverProof5.CompareTwoBytes(serverProof5Target));

        var tlvItem5s = new List<TlvItem>();
        tlvItem5s.Add(new TlvItem(new byte[]{(byte)Hap_Tlv_Tags.USERNAME},mac));
        tlvItem5s.Add(new TlvItem(new byte[]{(byte)Hap_Tlv_Tags.PUBLIC_KEY},serverPublic));
        tlvItem5s.Add(new TlvItem(new byte[]{(byte)Hap_Tlv_Tags.PROOF},serverProof5));
        var message5 = new Tlv().Encode(tlvItem5s);
        var message5Target = "'1,17,48,56,58,56,54,58,57,51,58,52,65,58,69,70,58,54,49,3,32,104,221,166,94,91,70,48,214,207,179,29,135,10,237,70,202,128,41,210,250,224,173,229,160,185,241,46,11,27,196,92,1,10,64,4,146,164,224,76,122,165,93,190,143,212,236,31,242,163,173,103,116,78,185,55,237,85,205,71,3,98,197,112,57,252,96,72,173,220,122,21,245,201,85,178,27,76,98,136,55,47,96,14,239,184,10,40,16,221,15,178,178,5,169,126,127,203,12'".GetSpecialStrToBytes();
        Assert.True(message5.CompareTwoBytes(message5Target));
        
        var PAIRING_5_NONCE = Utils.PadTlsNonce("PS-Msg06".ToBytes());
        var PAIRING_5_NONCETarget = "'0,0,0,0,80,83,45,77,115,103,48,54'".GetSpecialStrToBytes();
        Assert.True(PAIRING_5_NONCE.CompareTwoBytes(PAIRING_5_NONCETarget));
        var decryptedData5 = AeadAlgorithm.ChaCha20Poly1305.Encrypt(k, PAIRING_5_NONCE, new byte[0], message5);
        var decryptedData5Target = "'167,145,103,2,115,96,12,205,47,236,136,16,79,1,156,199,30,201,4,201,202,192,223,100,189,26,106,151,92,86,39,252,31,178,163,172,49,63,253,166,71,60,188,249,48,177,201,154,72,245,155,115,219,140,74,62,226,50,223,183,151,135,235,61,135,84,32,127,102,52,225,252,15,118,57,56,141,1,102,87,14,222,93,83,164,87,20,0,238,210,26,3,87,78,56,2,198,111,199,82,225,81,158,116,244,159,9,197,157,131,201,83,158,160,93,108,78,149,148,106,1,183,132,38,216,161,61,128,240,196,89,215,249,222,13'".GetSpecialStrToBytes();
        Assert.True(decryptedData5.CompareTwoBytes(decryptedData5Target));

        var clientUsernameStr = clientUserName.Value.GetString();
        Assert.Equal("EE0056F5-11BF-4236-9612-22B66851FB0B",clientUsernameStr);

        var clientGuid = new Guid(clientUsernameStr);
        
        var tlvItem6 = new List<TlvItem>();
        tlvItem6.Add(new TlvItem(new byte[]{(byte)Hap_Tlv_Tags.SEQUENCE_NUM},new byte[]{(byte)Hap_Tlv_States.M6}));
        tlvItem6.Add(new TlvItem(new byte[]{(byte)Hap_Tlv_Tags.ENCRYPTED_DATA},decryptedData5));
        var message6 = new Tlv().Encode(tlvItem6);
        var message6Target = "'6,1,6,5,135,167,145,103,2,115,96,12,205,47,236,136,16,79,1,156,199,30,201,4,201,202,192,223,100,189,26,106,151,92,86,39,252,31,178,163,172,49,63,253,166,71,60,188,249,48,177,201,154,72,245,155,115,219,140,74,62,226,50,223,183,151,135,235,61,135,84,32,127,102,52,225,252,15,118,57,56,141,1,102,87,14,222,93,83,164,87,20,0,238,210,26,3,87,78,56,2,198,111,199,82,225,81,158,116,244,159,9,197,157,131,201,83,158,160,93,108,78,149,148,106,1,183,132,38,216,161,61,128,240,196,89,215,249,222,13'".GetSpecialStrToBytes();
        Assert.True(message6.CompareTwoBytes(message6Target));
    }

    [Fact]
    public void TestPairVerifyTwo()
    {
        var encryptedDataBytes = "'113,5,250,168,249,4,253,99,211,249,170,49,231,138,41,70,111,65,174,141,193,226,240,229,193,168,100,96,46,215,168,166,5,196,251,143,17,207,171,71,173,126,18,87,137,145,229,209,5,225,229,83,134,176,251,189,54,246,102,1,134,230,30,219,210,36,162,172,187,99,15,128,23,5,249,226,120,220,164,0,217,188,205,242,231,235,89,139,155,201,231,189,251,53,139,254,115,209,148,138,21,73,234,47,128,147,113,40,175,184,95,135,119,208,69,190,197,165,180,13'".GetSpecialStrToBytes();
        var hkdf = "'48,43,74,189,92,122,143,218,124,3,9,222,105,98,197,84,196,118,66,114,186,184,194,220,179,241,27,185,245,242,188,172'".GetSpecialStrToBytes();
        using var k = Key.Import(AeadAlgorithm.ChaCha20Poly1305, hkdf, KeyBlobFormat.RawSymmetricKey);
        var decryptedData = AeadAlgorithm.ChaCha20Poly1305.Decrypt(k, Constants.PVERIFY_2_NONCE, new byte[0], encryptedDataBytes);
        var decryptedDataTarget = "'1,36,69,69,48,48,53,54,70,53,45,49,49,66,70,45,52,50,51,54,45,57,54,49,50,45,50,50,66,54,54,56,53,49,70,66,48,66,10,64,160,119,217,237,88,204,203,54,22,21,6,233,61,252,24,1,104,58,104,237,117,190,138,41,240,150,94,25,196,116,40,59,215,228,202,187,19,38,111,33,175,210,35,77,16,63,154,157,49,80,102,51,155,53,83,160,121,16,70,60,93,164,155,1'".GetSpecialStrToBytes();
        Assert.True(decryptedData.CompareTwoBytes(decryptedDataTarget));
        
        var tlvItems= new Tlv().Decode(decryptedData);
        var clientUserNameItem=tlvItems.FirstOrDefault(it => it.Tag[0]==(byte)Hap_Tlv_Tags.USERNAME);
        var ClientProof=tlvItems.FirstOrDefault(it => it.Tag[0]==(byte)Hap_Tlv_Tags.PROOF);
        var clientUserNameTarget = "'69,69,48,48,53,54,70,53,45,49,49,66,70,45,52,50,51,54,45,57,54,49,50,45,50,50,66,54,54,56,53,49,70,66,48,66'".GetSpecialStrToBytes();
        Assert.True(clientUserNameItem.Value.CompareTwoBytes(clientUserNameTarget));

        var clientPublic = "'185,16,153,136,24,206,57,106,36,198,137,31,58,161,193,173,4,105,159,113,116,82,140,187,251,84,179,172,93,251,6,87'".GetSpecialStrToBytes();
        var publicKey= "'106,19,28,229,202,59,218,129,64,72,187,153,35,221,136,212,142,129,155,208,80,89,45,48,105,23,211,112,14,61,85,4'".GetSpecialStrToBytes();
        
        var material = Utils.MergeBytes(clientPublic, clientUserNameItem.Value, publicKey);
        var materialTarget = "'185,16,153,136,24,206,57,106,36,198,137,31,58,161,193,173,4,105,159,113,116,82,140,187,251,84,179,172,93,251,6,87,69,69,48,48,53,54,70,53,45,49,49,66,70,45,52,50,51,54,45,57,54,49,50,45,50,50,66,54,54,56,53,49,70,66,48,66,106,19,28,229,202,59,218,129,64,72,187,153,35,221,136,212,142,129,155,208,80,89,45,48,105,23,211,112,14,61,85,4'".GetSpecialStrToBytes();
        Assert.True(material.CompareTwoBytes(materialTarget));

        var permClientPublic = "'203,191,202,190,130,36,28,52,50,26,157,80,24,70,93,239,151,221,213,194,111,119,254,106,191,83,243,50,101,33,38,33'".GetSpecialStrToBytes();
        
        var ed25519Key = PublicKey.Import(Ed25519.Ed25519, permClientPublic, KeyBlobFormat.RawPublicKey);
        var ed25519VerifyResult= Ed25519.Ed25519.Verify(ed25519Key, material, ClientProof.Value);
        Assert.True(ed25519VerifyResult);

        var tlvParams = new List<TlvItem>();
        tlvParams.Add(new TlvItem(new byte[]{(byte)Hap_Tlv_Tags.SEQUENCE_NUM},new byte[]{(byte)Hap_Tlv_States.M4}));
        var c2=new Tlv().Encode(tlvParams);
        var c2Target = "'6,1,4'".GetSpecialStrToBytes();
        Assert.True(c2.CompareTwoBytes(c2Target));
    }
    
    [Fact]
    public void TestPairVerifyOne()
    {
        var x25519KeyPair= Utils.GenerateX25519KeyPair();
        var otherPublicKeyBytes = "'171,70,179,160,131,242,27,78,95,22,60,52,65,135,105,236,222,69,199,117,155,170,134,162,109,185,200,88,41,54,120,3'".GetSpecialStrToBytes();
        var otherPublicKey = PublicKey.Import(X25519.X25519, otherPublicKeyBytes, KeyBlobFormat.RawPublicKey);
        var primaryKeyBytes = "'48,73,23,156,53,45,141,25,33,165,114,2,68,137,26,250,19,6,20,39,141,213,132,82,133,64,154,1,239,24,203,106'".GetSpecialStrToBytes();
        var  primaryKey= Key.Import(X25519.X25519, primaryKeyBytes, KeyBlobFormat.RawPrivateKey);
        var creationParameters = new SharedSecretCreationParameters()
        {
            ExportPolicy = KeyExportPolicies.AllowPlaintextArchiving
        };
        var sharedSecret= X25519.X25519.Agree( primaryKey,otherPublicKey,creationParameters );
        var sharedSExport = sharedSecret.Export(SharedSecretBlobFormat.RawSharedSecret);
        var sharedSExportTarget = "'53,74,174,136,116,120,74,40,65,139,101,119,249,6,36,89,126,66,29,248,12,12,4,132,251,138,26,113,121,38,76,105'".GetSpecialStrToBytes();
        Assert.True(sharedSExport.CompareTwoBytes(sharedSExportTarget));
        
        var mac= "'70,54,58,67,50,58,49,56,58,50,65,58,70,55,58,51,52'".GetSpecialStrToBytes();
        var publicKeyBytes = "'26,6,102,247,197,88,11,128,178,75,20,8,242,188,179,171,50,51,130,177,40,214,108,130,127,249,139,0,15,75,179,39'".GetSpecialStrToBytes();    
        var material = Utils.MergeBytes(publicKeyBytes, mac, otherPublicKeyBytes);
        var materialTarget = "'26,6,102,247,197,88,11,128,178,75,20,8,242,188,179,171,50,51,130,177,40,214,108,130,127,249,139,0,15,75,179,39,70,54,58,67,50,58,49,56,58,50,65,58,70,55,58,51,52,171,70,179,160,131,242,27,78,95,22,60,52,65,135,105,236,222,69,199,117,155,170,134,162,109,185,200,88,41,54,120,3'".GetSpecialStrToBytes();
       
        Assert.True(material.CompareTwoBytes(materialTarget));
        // var primaryKey5 = driver.State.PrivateKey;
        var ed25519PrimaryKey =
            "'72,232,163,181,84,55,101,48,163,40,216,244,233,199,94,98,124,209,54,177,85,181,142,84,57,110,14,70,195,25,226,188'"
                .GetSpecialStrToBytes();
        var ed25519Key5 = Key.Import(Ed25519.Ed25519, ed25519PrimaryKey, KeyBlobFormat.RawPrivateKey);
        var serverProof5= Ed25519.Ed25519.Sign(ed25519Key5, material);
        var serverProof5Target = "'252,174,235,177,184,6,135,137,38,114,29,14,9,66,146,178,97,124,29,212,0,112,201,246,28,248,239,143,173,225,99,25,108,206,59,188,189,115,25,101,8,190,193,122,56,108,200,26,244,253,178,48,186,163,71,242,1,74,163,144,20,168,159,3'".GetSpecialStrToBytes();
        Assert.True(serverProof5.CompareTwoBytes(serverProof5Target));
        var hkdfTarget =
            "'174,83,21,211,65,40,13,142,22,239,203,90,108,201,43,4,105,71,245,179,1,70,68,136,74,246,151,37,47,168,76,96'".GetSpecialStrToBytes();

        var hkdf = HKDF.DeriveKey(new HashAlgorithmName(nameof(SHA512)), sharedSExport, 32, Constants.PVERIFY_1_SALT, Constants.PVERIFY_1_INFO);
        
        Assert.True(hkdfTarget.CompareTwoBytes(hkdf));
        
        var tlvItem5s = new List<TlvItem>();
        tlvItem5s.Add(new TlvItem(new byte[]{(byte)Hap_Tlv_Tags.USERNAME},mac));
        tlvItem5s.Add(new TlvItem(new byte[]{(byte)Hap_Tlv_Tags.PROOF},serverProof5));
        var message5 = new Tlv().Encode(tlvItem5s);
        var message5Target = "'1,17,70,54,58,67,50,58,49,56,58,50,65,58,70,55,58,51,52,10,64,252,174,235,177,184,6,135,137,38,114,29,14,9,66,146,178,97,124,29,212,0,112,201,246,28,248,239,143,173,225,99,25,108,206,59,188,189,115,25,101,8,190,193,122,56,108,200,26,244,253,178,48,186,163,71,242,1,74,163,144,20,168,159,3'".GetSpecialStrToBytes();
        Assert.True(message5.CompareTwoBytes(message5Target));
        
        using var k = Key.Import(AeadAlgorithm.ChaCha20Poly1305, hkdf, KeyBlobFormat.RawSymmetricKey);
        var decryptedData5 = AeadAlgorithm.ChaCha20Poly1305.Encrypt(k, Constants.PVERIFY_1_NONCE, new byte[0], message5);
        var decryptedData5Target = "'171,54,162,205,9,104,205,7,28,15,121,40,58,127,48,111,20,147,158,44,216,188,12,1,55,188,107,14,54,49,117,37,203,52,240,34,6,6,217,125,80,189,128,248,194,145,9,200,173,141,9,172,29,189,158,219,102,101,233,85,88,193,232,106,119,81,247,160,241,111,246,45,181,203,237,50,197,140,215,154,69,46,120,255,199,35,233,4,172,44,175,54,44,99,210,231,172,244,147,87,12'".GetSpecialStrToBytes();
        Assert.True(decryptedData5.CompareTwoBytes(decryptedData5Target));
        
        var tlvItem6 = new List<TlvItem>();
        tlvItem6.Add(new TlvItem(new byte[]{(byte)Hap_Tlv_Tags.SEQUENCE_NUM},new byte[]{(byte)Hap_Tlv_States.M2}));
        tlvItem6.Add(new TlvItem(new byte[]{(byte)Hap_Tlv_Tags.ENCRYPTED_DATA},decryptedData5));
        tlvItem6.Add(new TlvItem(new byte[]{(byte)Hap_Tlv_Tags.PUBLIC_KEY},publicKeyBytes));
        var message6 = new Tlv().Encode(tlvItem6);
        var message6Target = "'6,1,2,5,101,171,54,162,205,9,104,205,7,28,15,121,40,58,127,48,111,20,147,158,44,216,188,12,1,55,188,107,14,54,49,117,37,203,52,240,34,6,6,217,125,80,189,128,248,194,145,9,200,173,141,9,172,29,189,158,219,102,101,233,85,88,193,232,106,119,81,247,160,241,111,246,45,181,203,237,50,197,140,215,154,69,46,120,255,199,35,233,4,172,44,175,54,44,99,210,231,172,244,147,87,12,3,32,26,6,102,247,197,88,11,128,178,75,20,8,242,188,179,171,50,51,130,177,40,214,108,130,127,249,139,0,15,75,179,39'".GetSpecialStrToBytes();
        Assert.True(message6.CompareTwoBytes(message6Target));
    }

}