﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedCDripLogChecker.Core.Checksum
{
    class State
    {
        public int Count { get; set; } = 0;
        public List<uint> AESState { get; set; } = new List<uint>() { 0, 0, 0, 0, 0, 0, 0, 0 };
        public byte[] Buffer { get; set; } = new byte[32];
    }

    public class EACAESChecker
    {
        private static readonly List<uint> T1 = new List<uint>() { 3332727651, 4169432188, 4003034999, 4136467323, 4279104242, 3602738027, 3736170351, 2438251973, 1615867952, 33751297, 3467208551, 1451043627, 3877240574, 3043153879, 1306962859, 3969545846, 2403715786, 530416258, 2302724553, 4203183485, 4011195130, 3001768281, 2395555655, 4211863792, 1106029997, 3009926356, 1610457762, 1173008303, 599760028, 1408738468, 3835064946, 2606481600, 1975695287, 3776773629, 1034851219, 1282024998, 1817851446, 2118205247, 4110612471, 2203045068, 1750873140, 1374987685, 3509904869, 4178113009, 3801313649, 2876496088, 1649619249, 708777237, 135005188, 2505230279, 1181033251, 2640233411, 807933976, 933336726, 168756485, 800430746, 235472647, 607523346, 463175808, 3745374946, 3441880043, 1315514151, 2144187058, 3936318837, 303761673, 496927619, 1484008492, 875436570, 908925723, 3702681198, 3035519578, 1543217312, 2767606354, 1984772923, 3076642518, 2110698419, 1383803177, 3711886307, 1584475951, 328696964, 2801095507, 3110654417, 0, 3240947181, 1080041504, 3810524412, 2043195825, 3069008731, 3569248874, 2370227147, 1742323390, 1917532473, 2497595978, 2564049996, 2968016984, 2236272591, 3144405200, 3307925487, 1340451498, 3977706491, 2261074755, 2597801293, 1716859699, 294946181, 2328839493, 3910203897, 67502594, 4269899647, 2700103760, 2017737788, 632987551, 1273211048, 2733855057, 1576969123, 2160083008, 92966799, 1068339858, 566009245, 1883781176, 4043634165, 1675607228, 2009183926, 2943736538, 1113792801, 540020752, 3843751935, 4245615603, 3211645650, 2169294285, 403966988, 641012499, 3274697964, 3202441055, 899848087, 2295088196, 775493399, 2472002756, 1441965991, 4236410494, 2051489085, 3366741092, 3135724893, 841685273, 3868554099, 3231735904, 429425025, 2664517455, 2743065820, 1147544098, 1417554474, 1001099408, 193169544, 2362066502, 3341414126, 1809037496, 675025940, 2809781982, 3168951902, 371002123, 2910247899, 3678134496, 1683370546, 1951283770, 337512970, 2463844681, 201983494, 1215046692, 3101973596, 2673722050, 3178157011, 1139780780, 3299238498, 967348625, 832869781, 3543655652, 4069226873, 3576883175, 2336475336, 1851340599, 3669454189, 25988493, 2976175573, 2631028302, 1239460265, 3635702892, 2902087254, 4077384948, 3475368682, 3400492389, 4102978170, 1206496942, 270010376, 1876277946, 4035475576, 1248797989, 1550986798, 941890588, 1475454630, 1942467764, 2538718918, 3408128232, 2709315037, 3902567540, 1042358047, 2531085131, 1641856445, 226921355, 260409994, 3767562352, 2084716094, 1908716981, 3433719398, 2430093384, 100991747, 4144101110, 470945294, 3265487201, 1784624437, 2935576407, 1775286713, 395413126, 2572730817, 975641885, 666476190, 3644383713, 3943954680, 733190296, 573772049, 3535497577, 2842745305, 126455438, 866620564, 766942107, 1008868894, 361924487, 3374377449, 2269761230, 2868860245, 1350051880, 2776293343, 59739276, 1509466529, 159418761, 437718285, 1708834751, 3610371814, 2227585602, 3501746280, 2193834305, 699439513, 1517759789, 504434447, 2076946608, 2835108948, 1842789307, 742004246 };
        private static readonly List<uint> T2 = new List<uint>() { 1673962851, 2096661628, 2012125559, 2079755643, 4076801522, 1809235307, 1876865391, 3314635973, 811618352, 16909057, 1741597031, 727088427, 4276558334, 3618988759, 2874009259, 1995217526, 3398387146, 2183110018, 3381215433, 2113570685, 4209972730, 1504897881, 1200539975, 4042984432, 2906778797, 3568527316, 2724199842, 2940594863, 2619588508, 2756966308, 1927583346, 3231407040, 3077948087, 4259388669, 2470293139, 642542118, 913070646, 1065238847, 4160029431, 3431157708, 879254580, 2773611685, 3855693029, 4059629809, 1910674289, 3635114968, 828527409, 355090197, 67636228, 3348452039, 591815971, 3281870531, 405809176, 2520228246, 84545285, 2586817946, 118360327, 304363026, 2149292928, 3806281186, 3956090603, 659450151, 2994720178, 1978310517, 152181513, 2199756419, 743994412, 439627290, 456535323, 1859957358, 1521806938, 2690382752, 1386542674, 997608763, 3602342358, 3011366579, 693271337, 3822927587, 794718511, 2215876484, 1403450707, 3518589137, 0, 3988860141, 541089824, 4242743292, 2977548465, 1538714971, 1792327274, 3415033547, 3194476990, 963791673, 1251270218, 1285084236, 1487988824, 3481619151, 3501943760, 4022676207, 2857362858, 4226619131, 1132905795, 1301993293, 862344499, 2232521861, 1166724933, 4192801017, 33818114, 2147385727, 1352724560, 1014514748, 2670049951, 2823545768, 1369633617, 2740846243, 1082179648, 2399505039, 2453646738, 2636233885, 946882616, 4126213365, 3160661948, 3061301686, 3668932058, 557998881, 270544912, 4293204735, 4093447923, 3535760850, 3447803085, 202904588, 321271059, 3972214764, 1606345055, 2536874647, 1149815876, 388905239, 3297990596, 2807427751, 2130477694, 1031423805, 1690872932, 1572530013, 422718233, 1944491379, 1623236704, 2165938305, 1335808335, 3701702620, 574907938, 710180394, 2419829648, 2282455944, 1183631942, 4006029806, 3094074296, 338181140, 3735517662, 1589437022, 185998603, 3685578459, 3772464096, 845436466, 980700730, 169090570, 1234361161, 101452294, 608726052, 1555620956, 3265224130, 3552407251, 2890133420, 1657054818, 2436475025, 2503058581, 3839047652, 2045938553, 3889509095, 3364570056, 929978679, 1843050349, 2365688973, 3585172693, 1318900302, 2840191145, 1826141292, 1454176854, 4109567988, 3939444202, 1707781989, 2062847610, 2923948462, 135272456, 3127891386, 2029029496, 625635109, 777810478, 473441308, 2790781350, 3027486644, 3331805638, 3905627112, 3718347997, 1961401460, 524165407, 1268178251, 3177307325, 2332919435, 2316273034, 1893765232, 1048330814, 3044132021, 1724688998, 1217452104, 50726147, 4143383030, 236720654, 1640145761, 896163637, 1471084887, 3110719673, 2249691526, 3248052417, 490350365, 2653403550, 3789109473, 4176155640, 2553000856, 287453969, 1775418217, 3651760345, 2382858638, 2486413204, 2603464347, 507257374, 2266337927, 3922272489, 3464972750, 1437269845, 676362280, 3752164063, 2349043596, 2707028129, 2299101321, 219813645, 3211123391, 3872862694, 1115997762, 1758509160, 1099088705, 2569646233, 760903469, 253628687, 2960903088, 1420360788, 3144537787, 371997206 };
        private static readonly List<uint> T3 = new List<uint>() { 1667483301, 2088564868, 2004348569, 2071721613, 4076011277, 1802229437, 1869602481, 3318059348, 808476752, 16843267, 1734856361, 724260477, 4278118169, 3621238114, 2880130534, 1987505306, 3402272581, 2189565853, 3385428288, 2105408135, 4210749205, 1499050731, 1195871945, 4042324747, 2913812972, 3570709351, 2728550397, 2947499498, 2627478463, 2762232823, 1920132246, 3233848155, 3082253762, 4261273884, 2475900334, 640044138, 909536346, 1061125697, 4160222466, 3435955023, 875849820, 2779075060, 3857043764, 4059166984, 1903288979, 3638078323, 825320019, 353708607, 67373068, 3351745874, 589514341, 3284376926, 404238376, 2526427041, 84216335, 2593796021, 117902857, 303178806, 2155879323, 3806519101, 3958099238, 656887401, 2998042573, 1970662047, 151589403, 2206408094, 741103732, 437924910, 454768173, 1852759218, 1515893998, 2694863867, 1381147894, 993752653, 3604395873, 3014884814, 690573947, 3823361342, 791633521, 2223248279, 1397991157, 3520182632, 0, 3991781676, 538984544, 4244431647, 2981198280, 1532737261, 1785386174, 3419114822, 3200149465, 960066123, 1246401758, 1280088276, 1482207464, 3486483786, 3503340395, 4025468202, 2863288293, 4227591446, 1128498885, 1296931543, 859006549, 2240090516, 1162185423, 4193904912, 33686534, 2139094657, 1347461360, 1010595908, 2678007226, 2829601763, 1364304627, 2745392638, 1077969088, 2408514954, 2459058093, 2644320700, 943222856, 4126535940, 3166462943, 3065411521, 3671764853, 555827811, 269492272, 4294960410, 4092853518, 3537026925, 3452797260, 202119188, 320022069, 3974939439, 1600110305, 2543269282, 1145342156, 387395129, 3301217111, 2812761586, 2122251394, 1027439175, 1684326572, 1566423783, 421081643, 1936975509, 1616953504, 2172721560, 1330618065, 3705447295, 572671078, 707417214, 2425371563, 2290617219, 1179028682, 4008625961, 3099093971, 336865340, 3739133817, 1583267042, 185275933, 3688607094, 3772832571, 842163286, 976909390, 168432670, 1229558491, 101059594, 606357612, 1549580516, 3267534685, 3553869166, 2896970735, 1650640038, 2442213800, 2509582756, 3840201527, 2038035083, 3890730290, 3368586051, 926379609, 1835915959, 2374828428, 3587551588, 1313774802, 2846444000, 1819072692, 1448520954, 4109693703, 3941256997, 1701169839, 2054878350, 2930657257, 134746136, 3132780501, 2021191816, 623200879, 774790258, 471611428, 2795919345, 3031724999, 3334903633, 3907570467, 3722289532, 1953818780, 522141217, 1263245021, 3183305180, 2341145990, 2324303749, 1886445712, 1044282434, 3048567236, 1718013098, 1212715224, 50529797, 4143380225, 235805714, 1633796771, 892693087, 1465364217, 3115936208, 2256934801, 3250690392, 488454695, 2661164985, 3789674808, 4177062675, 2560109491, 286335539, 1768542907, 3654920560, 2391672713, 2492740519, 2610638262, 505297954, 2273777042, 3924412704, 3469641545, 1431677695, 673730680, 3755976058, 2357986191, 2711706104, 2307459456, 218962455, 3216991706, 3873888049, 1111655622, 1751699640, 1094812355, 2576951728, 757946999, 252648977, 2964356043, 1414834428, 3149622742, 370551866 };
        private static readonly List<uint> T4 = new List<uint>() { 2774754246, 2222750968, 2574743534, 2373680118, 234025727, 3177933782, 2976870366, 1422247313, 1345335392, 50397442, 2842126286, 2099981142, 436141799, 1658312629, 3870010189, 2591454956, 1170918031, 2642575903, 1086966153, 2273148410, 368769775, 3948501426, 3376891790, 200339707, 3970805057, 1742001331, 4255294047, 3937382213, 3214711843, 4154762323, 2524082916, 1539358875, 3266819957, 486407649, 2928907069, 1780885068, 1513502316, 1094664062, 49805301, 1338821763, 1546925160, 4104496465, 887481809, 150073849, 2473685474, 1943591083, 1395732834, 1058346282, 201589768, 1388824469, 1696801606, 1589887901, 672667696, 2711000631, 251987210, 3046808111, 151455502, 907153956, 2608889883, 1038279391, 652995533, 1764173646, 3451040383, 2675275242, 453576978, 2659418909, 1949051992, 773462580, 756751158, 2993581788, 3998898868, 4221608027, 4132590244, 1295727478, 1641469623, 3467883389, 2066295122, 1055122397, 1898917726, 2542044179, 4115878822, 1758581177, 0, 753790401, 1612718144, 536673507, 3367088505, 3982187446, 3194645204, 1187761037, 3653156455, 1262041458, 3729410708, 3561770136, 3898103984, 1255133061, 1808847035, 720367557, 3853167183, 385612781, 3309519750, 3612167578, 1429418854, 2491778321, 3477423498, 284817897, 100794884, 2172616702, 4031795360, 1144798328, 3131023141, 3819481163, 4082192802, 4272137053, 3225436288, 2324664069, 2912064063, 3164445985, 1211644016, 83228145, 3753688163, 3249976951, 1977277103, 1663115586, 806359072, 452984805, 250868733, 1842533055, 1288555905, 336333848, 890442534, 804056259, 3781124030, 2727843637, 3427026056, 957814574, 1472513171, 4071073621, 2189328124, 1195195770, 2892260552, 3881655738, 723065138, 2507371494, 2690670784, 2558624025, 3511635870, 2145180835, 1713513028, 2116692564, 2878378043, 2206763019, 3393603212, 703524551, 3552098411, 1007948840, 2044649127, 3797835452, 487262998, 1994120109, 1004593371, 1446130276, 1312438900, 503974420, 3679013266, 168166924, 1814307912, 3831258296, 1573044895, 1859376061, 4021070915, 2791465668, 2828112185, 2761266481, 937747667, 2339994098, 854058965, 1137232011, 1496790894, 3077402074, 2358086913, 1691735473, 3528347292, 3769215305, 3027004632, 4199962284, 133494003, 636152527, 2942657994, 2390391540, 3920539207, 403179536, 3585784431, 2289596656, 1864705354, 1915629148, 605822008, 4054230615, 3350508659, 1371981463, 602466507, 2094914977, 2624877800, 555687742, 3712699286, 3703422305, 2257292045, 2240449039, 2423288032, 1111375484, 3300242801, 2858837708, 3628615824, 84083462, 32962295, 302911004, 2741068226, 1597322602, 4183250862, 3501832553, 2441512471, 1489093017, 656219450, 3114180135, 954327513, 335083755, 3013122091, 856756514, 3144247762, 1893325225, 2307821063, 2811532339, 3063651117, 572399164, 2458355477, 552200649, 1238290055, 4283782570, 2015897680, 2061492133, 2408352771, 4171342169, 2156497161, 386731290, 3669999461, 837215959, 3326231172, 3093850320, 3275833730, 2962856233, 1999449434, 286199582, 3417354363, 4233385128, 3602627437, 974525996 };
        private static readonly List<uint> T5 = new List<uint>() { 0x63, 0x7C, 0x77, 0x7B, 0xF2, 0x6B, 0x6F, 0xC5, 0x30, 0x01, 0x67, 0x2B, 0xFE, 0xD7, 0xAB, 0x76, 0xCA, 0x82, 0xC9, 0x7D, 0xFA, 0x59, 0x47, 0xF0, 0xAD, 0xD4, 0xA2, 0xAF, 0x9C, 0xA4, 0x72, 0xC0, 0xB7, 0xFD, 0x93, 0x26, 0x36, 0x3F, 0xF7, 0xCC, 0x34, 0xA5, 0xE5, 0xF1, 0x71, 0xD8, 0x31, 0x15, 0x04, 0xC7, 0x23, 0xC3, 0x18, 0x96, 0x05, 0x9A, 0x07, 0x12, 0x80, 0xE2, 0xEB, 0x27, 0xB2, 0x75, 0x09, 0x83, 0x2C, 0x1A, 0x1B, 0x6E, 0x5A, 0xA0, 0x52, 0x3B, 0xD6, 0xB3, 0x29, 0xE3, 0x2F, 0x84, 0x53, 0xD1, 0x00, 0xED, 0x20, 0xFC, 0xB1, 0x5B, 0x6A, 0xCB, 0xBE, 0x39, 0x4A, 0x4C, 0x58, 0xCF, 0xD0, 0xEF, 0xAA, 0xFB, 0x43, 0x4D, 0x33, 0x85, 0x45, 0xF9, 0x02, 0x7F, 0x50, 0x3C, 0x9F, 0xA8, 0x51, 0xA3, 0x40, 0x8F, 0x92, 0x9D, 0x38, 0xF5, 0xBC, 0xB6, 0xDA, 0x21, 0x10, 0xFF, 0xF3, 0xD2, 0xCD, 0x0C, 0x13, 0xEC, 0x5F, 0x97, 0x44, 0x17, 0xC4, 0xA7, 0x7E, 0x3D, 0x64, 0x5D, 0x19, 0x73, 0x60, 0x81, 0x4F, 0xDC, 0x22, 0x2A, 0x90, 0x88, 0x46, 0xEE, 0xB8, 0x14, 0xDE, 0x5E, 0x0B, 0xDB, 0xE0, 0x32, 0x3A, 0x0A, 0x49, 0x06, 0x24, 0x5C, 0xC2, 0xD3, 0xAC, 0x62, 0x91, 0x95, 0xE4, 0x79, 0xE7, 0xC8, 0x37, 0x6D, 0x8D, 0xD5, 0x4E, 0xA9, 0x6C, 0x56, 0xF4, 0xEA, 0x65, 0x7A, 0xAE, 0x08, 0xBA, 0x78, 0x25, 0x2E, 0x1C, 0xA6, 0xB4, 0xC6, 0xE8, 0xDD, 0x74, 0x1F, 0x4B, 0xBD, 0x8B, 0x8A, 0x70, 0x3E, 0xB5, 0x66, 0x48, 0x03, 0xF6, 0x0E, 0x61, 0x35, 0x57, 0xB9, 0x86, 0xC1, 0x1D, 0x9E, 0xE1, 0xF8, 0x98, 0x11, 0x69, 0xD9, 0x8E, 0x94, 0x9B, 0x1E, 0x87, 0xE9, 0xCE, 0x55, 0x28, 0xDF, 0x8C, 0xA1, 0x89, 0x0D, 0xBF, 0xE6, 0x42, 0x68, 0x41, 0x99, 0x2D, 0x0F, 0xB0, 0x54, 0xBB, 0x16 };
        private static readonly List<uint> AESStuff = new List<uint>() { 1819375763, 1698840305, 2385728942, 1630997396, 2654526084, 645358672, 879714227, 3656377118, 507815811, 2064030066, 4113943772, 2483210056, 3159301334, 2587392134, 2924947253, 2008597547, 4021406621, 2495042287, 1635950131, 4118829435, 1518524407, 3233460081, 1861060676, 425020527, 1197815728, 3553781087, 2991651692, 1204855319, 4210224391, 977814134, 1420151346, 1307608669, 193379324, 3629440675, 1778656719, 768903128, 582679142, 418560016, 1280799266, 27784319, 3657099091, 28195312, 1806310463, 1182702567, 2018111218, 1622890210, 753673408, 759943359, 3508687875, 3498864115, 3139858892, 4250462763, 740518147, 1285046241, 1618221857, 1296026526, 3670090076, 172838063, 2976595299, 1278465864, 98592593, 1232623792, 688669585, 1681098767, 2894258712, 2798628535, 396661716, 1536640156, 1013480591, 1963992127, 1545365422, 942665633, 2659579868, 944396651, 804178623, 1954123299, 2933810601, 3687727510, 2278696504, 3221080473, 1888330551, 1221065308, 1730752739, 324058816, 3549285139, 138644101, 2408688829, 812410148, 1183445782, 239995210, 1768372649, 2050459497, 152558826, 22304367, 2395339474, 3198787574, 69672981, 174664031, 1661866230, 423204767, 3708113969, 3696296542, 1385494668, 3963447162, 3739813691, 3565170276, 3079552658, 2931271949, 967608294, 3858597304, 3077143860, 1532238414, 4056950740, 626002352, 2464036642, 1013554735, 3528822003, 933759307, 2160161919, 3684003377 };

        private static int BYTE0(uint n) => int.Parse(((n & 0x000000FF) >> 0).ToString());
        private static int BYTE1(uint n) => int.Parse(((n & 0x0000FF00) >> 8).ToString());
        private static int BYTE2(uint n) => int.Parse(((n & 0x00FF0000) >> 16).ToString());
        private static int BYTE3(uint n) => int.Parse(((n & 0xFF000000) >> 24).ToString());

        private static State GetAES(State state)
        {
            var r = 14;

            var a1 = state.AESState[0] ^ AESStuff[0];
            var a2 = state.AESState[1] ^ AESStuff[1];
            var a3 = state.AESState[2] ^ AESStuff[2];
            var a4 = state.AESState[3] ^ AESStuff[3];
            var a5 = state.AESState[4] ^ AESStuff[4];
            var a6 = state.AESState[5] ^ AESStuff[5];
            var a7 = state.AESState[6] ^ AESStuff[6];
            var a8 = state.AESState[7] ^ AESStuff[7];

            var b1 = AESStuff[8 * r - 104] ^ T1[BYTE3(a5)] ^ T2[BYTE2(a4)] ^ T3[BYTE1(a2)] ^ T4[BYTE0(a1)];
            var b2 = AESStuff[8 * r - 103] ^ T1[BYTE3(a6)] ^ T2[BYTE2(a5)] ^ T3[BYTE1(a3)] ^ T4[BYTE0(a2)];
            var b3 = AESStuff[8 * r - 102] ^ T1[BYTE3(a7)] ^ T2[BYTE2(a6)] ^ T3[BYTE1(a4)] ^ T4[BYTE0(a3)];
            var b4 = AESStuff[8 * r - 101] ^ T1[BYTE3(a8)] ^ T2[BYTE2(a7)] ^ T3[BYTE1(a5)] ^ T4[BYTE0(a4)];
            var b5 = AESStuff[8 * r - 100] ^ T1[BYTE3(a1)] ^ T2[BYTE2(a8)] ^ T3[BYTE1(a6)] ^ T4[BYTE0(a5)];
            var b6 = AESStuff[8 * r - 99] ^ T1[BYTE3(a2)] ^ T2[BYTE2(a1)] ^ T3[BYTE1(a7)] ^ T4[BYTE0(a6)];
            var b7 = AESStuff[8 * r - 98] ^ T1[BYTE3(a3)] ^ T2[BYTE2(a2)] ^ T3[BYTE1(a8)] ^ T4[BYTE0(a7)];
            var b8 = AESStuff[8 * r - 97] ^ T1[BYTE3(a4)] ^ T2[BYTE2(a3)] ^ T3[BYTE1(a1)] ^ T4[BYTE0(a8)];

            var c1 = AESStuff[8 * r - 96] ^ T1[BYTE3(b5)] ^ T2[BYTE2(b4)] ^ T3[BYTE1(b2)] ^ T4[BYTE0(b1)];
            var c2 = AESStuff[8 * r - 95] ^ T1[BYTE3(b6)] ^ T2[BYTE2(b5)] ^ T3[BYTE1(b3)] ^ T4[BYTE0(b2)];
            var c3 = AESStuff[8 * r - 94] ^ T1[BYTE3(b7)] ^ T2[BYTE2(b6)] ^ T3[BYTE1(b4)] ^ T4[BYTE0(b3)];
            var c4 = AESStuff[8 * r - 93] ^ T1[BYTE3(b8)] ^ T2[BYTE2(b7)] ^ T3[BYTE1(b5)] ^ T4[BYTE0(b4)];
            var c5 = AESStuff[8 * r - 92] ^ T1[BYTE3(b1)] ^ T2[BYTE2(b8)] ^ T3[BYTE1(b6)] ^ T4[BYTE0(b5)];
            var c6 = AESStuff[8 * r - 91] ^ T1[BYTE3(b2)] ^ T2[BYTE2(b1)] ^ T3[BYTE1(b7)] ^ T4[BYTE0(b6)];
            var c7 = AESStuff[8 * r - 90] ^ T1[BYTE3(b3)] ^ T2[BYTE2(b2)] ^ T3[BYTE1(b8)] ^ T4[BYTE0(b7)];
            var c8 = AESStuff[8 * r - 89] ^ T1[BYTE3(b4)] ^ T2[BYTE2(b3)] ^ T3[BYTE1(b1)] ^ T4[BYTE0(b8)];

            var c9 = AESStuff[8 * r - 88] ^ T1[BYTE3(c5)] ^ T2[BYTE2(c4)] ^ T3[BYTE1(c2)] ^ T4[BYTE0(c1)];
            var ca = AESStuff[8 * r - 87] ^ T1[BYTE3(c6)] ^ T2[BYTE2(c5)] ^ T3[BYTE1(c3)] ^ T4[BYTE0(c2)];
            var cb = AESStuff[8 * r - 86] ^ T1[BYTE3(c7)] ^ T2[BYTE2(c6)] ^ T3[BYTE1(c4)] ^ T4[BYTE0(c3)];
            var cc = AESStuff[8 * r - 85] ^ T1[BYTE3(c8)] ^ T2[BYTE2(c7)] ^ T3[BYTE1(c5)] ^ T4[BYTE0(c4)];
            var cd = AESStuff[8 * r - 84] ^ T1[BYTE3(c1)] ^ T2[BYTE2(c8)] ^ T3[BYTE1(c6)] ^ T4[BYTE0(c5)];
            var ce = AESStuff[8 * r - 83] ^ T1[BYTE3(c2)] ^ T2[BYTE2(c1)] ^ T3[BYTE1(c7)] ^ T4[BYTE0(c6)];
            var cf = AESStuff[8 * r - 82] ^ T1[BYTE3(c3)] ^ T2[BYTE2(c2)] ^ T3[BYTE1(c8)] ^ T4[BYTE0(c7)];
            var cg = AESStuff[8 * r - 81] ^ T1[BYTE3(c4)] ^ T2[BYTE2(c3)] ^ T3[BYTE1(c1)] ^ T4[BYTE0(c8)];

            for (int i = 0; i < 5; i++)
            {
                var d1 = AESStuff[8 * r - 80 + 16 * i] ^ T1[BYTE3(cd)] ^ T2[BYTE2(cc)] ^ T3[BYTE1(ca)] ^ T4[BYTE0(c9)];
                var d2 = AESStuff[8 * r - 79 + 16 * i] ^ T1[BYTE3(ce)] ^ T2[BYTE2(cd)] ^ T3[BYTE1(cb)] ^ T4[BYTE0(ca)];
                var d3 = AESStuff[8 * r - 78 + 16 * i] ^ T1[BYTE3(cf)] ^ T2[BYTE2(ce)] ^ T3[BYTE1(cc)] ^ T4[BYTE0(cb)];
                var d4 = AESStuff[8 * r - 77 + 16 * i] ^ T1[BYTE3(cg)] ^ T2[BYTE2(cf)] ^ T3[BYTE1(cd)] ^ T4[BYTE0(cc)];
                var d5 = AESStuff[8 * r - 76 + 16 * i] ^ T1[BYTE3(c9)] ^ T2[BYTE2(cg)] ^ T3[BYTE1(ce)] ^ T4[BYTE0(cd)];
                var d6 = AESStuff[8 * r - 75 + 16 * i] ^ T1[BYTE3(ca)] ^ T2[BYTE2(c9)] ^ T3[BYTE1(cf)] ^ T4[BYTE0(ce)];
                var d7 = AESStuff[8 * r - 74 + 16 * i] ^ T1[BYTE3(cb)] ^ T2[BYTE2(ca)] ^ T3[BYTE1(cg)] ^ T4[BYTE0(cf)];
                var d8 = AESStuff[8 * r - 73 + 16 * i] ^ T1[BYTE3(cc)] ^ T2[BYTE2(cb)] ^ T3[BYTE1(c9)] ^ T4[BYTE0(cg)];

                var d9 = AESStuff[8 * r - 72 + 16 * i] ^ T1[BYTE3(d5)] ^ T2[BYTE2(d4)] ^ T3[BYTE1(d2)] ^ T4[BYTE0(d1)];
                var da = AESStuff[8 * r - 71 + 16 * i] ^ T1[BYTE3(d6)] ^ T2[BYTE2(d5)] ^ T3[BYTE1(d3)] ^ T4[BYTE0(d2)];
                var db = AESStuff[8 * r - 70 + 16 * i] ^ T1[BYTE3(d7)] ^ T2[BYTE2(d6)] ^ T3[BYTE1(d4)] ^ T4[BYTE0(d3)];
                var dc = AESStuff[8 * r - 69 + 16 * i] ^ T1[BYTE3(d8)] ^ T2[BYTE2(d7)] ^ T3[BYTE1(d5)] ^ T4[BYTE0(d4)];
                var dd = AESStuff[8 * r - 68 + 16 * i] ^ T1[BYTE3(d1)] ^ T2[BYTE2(d8)] ^ T3[BYTE1(d6)] ^ T4[BYTE0(d5)];
                var de = AESStuff[8 * r - 67 + 16 * i] ^ T1[BYTE3(d2)] ^ T2[BYTE2(d1)] ^ T3[BYTE1(d7)] ^ T4[BYTE0(d6)];
                var df = AESStuff[8 * r - 66 + 16 * i] ^ T1[BYTE3(d3)] ^ T2[BYTE2(d2)] ^ T3[BYTE1(d8)] ^ T4[BYTE0(d7)];
                var dg = AESStuff[8 * r - 65 + 16 * i] ^ T1[BYTE3(d4)] ^ T2[BYTE2(d3)] ^ T3[BYTE1(d1)] ^ T4[BYTE0(d8)];

                c9 = d9;
                ca = da;
                cb = db;
                cc = dc;
                cd = dd;
                ce = de;
                cf = df;
                cg = dg;
            }

            state.AESState[0] = AESStuff[8 * r + 0] ^ (T5[BYTE0(c9)] | (T5[BYTE1(ca)] << 8) | (T5[BYTE2(cc)] << 16) | (T5[BYTE3(cd)] << 24));
            state.AESState[1] = AESStuff[8 * r + 1] ^ (T5[BYTE0(ca)] | (T5[BYTE1(cb)] << 8) | (T5[BYTE2(cd)] << 16) | (T5[BYTE3(ce)] << 24));
            state.AESState[2] = AESStuff[8 * r + 2] ^ (T5[BYTE0(cb)] | (T5[BYTE1(cc)] << 8) | (T5[BYTE2(ce)] << 16) | (T5[BYTE3(cf)] << 24));
            state.AESState[3] = AESStuff[8 * r + 3] ^ (T5[BYTE0(cc)] | (T5[BYTE1(cd)] << 8) | (T5[BYTE2(cf)] << 16) | (T5[BYTE3(cg)] << 24));
            state.AESState[4] = AESStuff[8 * r + 4] ^ (T5[BYTE0(cd)] | (T5[BYTE1(ce)] << 8) | (T5[BYTE2(cg)] << 16) | (T5[BYTE3(c9)] << 24));
            state.AESState[5] = AESStuff[8 * r + 5] ^ (T5[BYTE0(ce)] | (T5[BYTE1(cf)] << 8) | (T5[BYTE2(c9)] << 16) | (T5[BYTE3(ca)] << 24));
            state.AESState[6] = AESStuff[8 * r + 6] ^ (T5[BYTE0(cf)] | (T5[BYTE1(cg)] << 8) | (T5[BYTE2(ca)] << 16) | (T5[BYTE3(cb)] << 24));
            state.AESState[7] = AESStuff[8 * r + 7] ^ (T5[BYTE0(cg)] | (T5[BYTE1(c9)] << 8) | (T5[BYTE2(cb)] << 16) | (T5[BYTE3(cc)] << 24));

            return state;
        }

        public static string ComputeChecksum(string log)
        {
            var state = new State();
            log = log.Split(new string[] { "\r\n\r\n==== Log checksum" }, StringSplitOptions.None)[0];
            var logs = log.Replace("\r", "").Replace("\n", "").ToList();
            foreach (var item in logs)
            {
                byte[] bts = BitConverter.GetBytes(item);
                for (int i = state.Count; i < state.Count + 2; i++)
                {
                    state.Buffer[i] = bts[i - state.Count];
                }
                state.Count += 2;
                if (state.Count == 32)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        byte[] buffers = new byte[4 * (i + 1)];
                        for (int j = 4*i; j < 4*(i+1); j++)
                        {
                            buffers[j - 4 * i] = state.Buffer[j];
                        }
                        state.AESState[i] ^= BitConverter.ToUInt32(buffers,0);
                    }
                    state = GetAES(state);
                    state.Count = 0;
                }
            }
            if (state.Count > 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    byte[] buffers = new byte[4 * (i + 1)];
                    for (int j = 4 * i; j < 4 * (i + 1); j++)
                    {
                        buffers[j - 4 * i] = state.Buffer[j];
                    }
                    state.AESState[i] ^= BitConverter.ToUInt32(buffers, 0);
                }
                state = GetAES(state);
            }
            List<byte> result = new List<byte>();
            foreach (var item in state.AESState)
            {
                result.AddRange(BitConverter.GetBytes(item));
            }
            return BitConverter.ToString(result.ToArray(),0).Replace("-", string.Empty).ToLower();
        }
    }

    class StructConverter
    {
        // We use this function to provide an easier way to type-agnostically call the GetBytes method of the BitConverter class.
        // This means we can have much cleaner code below.
        private static byte[] TypeAgnosticGetBytes(object o)
        {
            if (o is int) return BitConverter.GetBytes((int)o);
            if (o is uint) return BitConverter.GetBytes((uint)o);
            if (o is long) return BitConverter.GetBytes((long)o);
            if (o is ulong) return BitConverter.GetBytes((ulong)o);
            if (o is short) return BitConverter.GetBytes((short)o);
            if (o is ushort) return BitConverter.GetBytes((ushort)o);
            if (o is byte || o is sbyte) return new byte[] { (byte)o };
            throw new ArgumentException("Unsupported object type found");
        }

        private static string GetFormatSpecifierFor(object o)
        {
            if (o is int) return "i";
            if (o is uint) return "I";
            if (o is long) return "q";
            if (o is ulong) return "Q";
            if (o is short) return "h";
            if (o is ushort) return "H";
            if (o is byte) return "B";
            if (o is sbyte) return "b";
            throw new ArgumentException("Unsupported object type found");
        }

        /// <summary>
        /// Convert a byte array into an array of objects based on Python's "struct.unpack" protocol.
        /// </summary>
        /// <param name="fmt">A "struct.pack"-compatible format string</param>
        /// <param name="bytes">An array of bytes to convert to objects</param>
        /// <returns>Array of objects.</returns>
        /// <remarks>You are responsible for casting the objects in the array back to their proper types.</remarks>
        public static object[] Unpack(string fmt, byte[] bytes)
        {
            Debug.WriteLine("Format string is length {0}, {1} bytes provided.", fmt.Length, bytes.Length);

            // First we parse the format string to make sure it's proper.
            if (fmt.Length < 1) throw new ArgumentException("Format string cannot be empty.");

            bool endianFlip = false;
            if (fmt.Substring(0, 1) == "<")
            {
                Debug.WriteLine("  Endian marker found: little endian");
                // Little endian.
                // Do we need to flip endianness?
                if (BitConverter.IsLittleEndian == false) endianFlip = true;
                fmt = fmt.Substring(1);
            }
            else if (fmt.Substring(0, 1) == ">")
            {
                Debug.WriteLine("  Endian marker found: big endian");
                // Big endian.
                // Do we need to flip endianness?
                if (BitConverter.IsLittleEndian == true) endianFlip = true;
                fmt = fmt.Substring(1);
            }

            // Now, we find out how long the byte array needs to be
            int totalByteLength = 0;
            foreach (char c in fmt.ToCharArray())
            {
                Debug.WriteLine("  Format character found: {0}", c);
                switch (c)
                {
                    case 'q':
                    case 'Q':
                        totalByteLength += 8;
                        break;
                    case 'i':
                    case 'I':
                        totalByteLength += 4;
                        break;
                    case 'h':
                    case 'H':
                        totalByteLength += 2;
                        break;
                    case 'b':
                    case 'B':
                    case 'x':
                        totalByteLength += 1;
                        break;
                    default:
                        throw new ArgumentException("Invalid character found in format string.");
                }
            }

            Debug.WriteLine("Endianness will {0}be flipped.", (object)(endianFlip == true ? "" : "NOT "));
            Debug.WriteLine("The byte array is expected to be {0} bytes long.", totalByteLength);

            // Test the byte array length to see if it contains as many bytes as is needed for the string.
            if (bytes.Length != totalByteLength) throw new ArgumentException("The number of bytes provided does not match the total length of the format string.");

            // Ok, we can go ahead and start parsing bytes!
            int byteArrayPosition = 0;
            List<object> outputList = new List<object>();
            byte[] buf;

            Debug.WriteLine("Processing byte array...");
            foreach (char c in fmt.ToCharArray())
            {
                switch (c)
                {
                    case 'q':
                        outputList.Add((object)(long)BitConverter.ToInt64(bytes, byteArrayPosition));
                        byteArrayPosition += 8;
                        Debug.WriteLine("  Added signed 64-bit integer.");
                        break;
                    case 'Q':
                        outputList.Add((object)(ulong)BitConverter.ToUInt64(bytes, byteArrayPosition));
                        byteArrayPosition += 8;
                        Debug.WriteLine("  Added unsigned 64-bit integer.");
                        break;
                    case 'l':
                        outputList.Add((object)(int)BitConverter.ToInt32(bytes, byteArrayPosition));
                        byteArrayPosition += 4;
                        Debug.WriteLine("  Added signed 32-bit integer.");
                        break;
                    case 'L':
                        outputList.Add((object)(uint)BitConverter.ToUInt32(bytes, byteArrayPosition));
                        byteArrayPosition += 4;
                        Debug.WriteLine("  Added unsignedsigned 32-bit integer.");
                        break;
                    case 'h':
                        outputList.Add((object)(short)BitConverter.ToInt16(bytes, byteArrayPosition));
                        byteArrayPosition += 2;
                        Debug.WriteLine("  Added signed 16-bit integer.");
                        break;
                    case 'H':
                        outputList.Add((object)(ushort)BitConverter.ToUInt16(bytes, byteArrayPosition));
                        byteArrayPosition += 2;
                        Debug.WriteLine("  Added unsigned 16-bit integer.");
                        break;
                    case 'b':
                        buf = new byte[1];
                        Array.Copy(bytes, byteArrayPosition, buf, 0, 1);
                        outputList.Add((object)(sbyte)buf[0]);
                        byteArrayPosition++;
                        Debug.WriteLine("  Added signed byte");
                        break;
                    case 'B':
                        buf = new byte[1];
                        Array.Copy(bytes, byteArrayPosition, buf, 0, 1);
                        outputList.Add((object)(byte)buf[0]);
                        byteArrayPosition++;
                        Debug.WriteLine("  Added unsigned byte");
                        break;
                    case 'x':
                        byteArrayPosition++;
                        Debug.WriteLine("  Ignoring a byte");
                        break;
                    default:
                        throw new ArgumentException("You should not be here.");
                }
            }
            return outputList.ToArray();
        }

        /// <summary>
        /// Convert an array of objects to a byte array, along with a string that can be used with Unpack.
        /// </summary>
        /// <param name="items">An object array of items to convert</param>
        /// <param name="LittleEndian">Set to False if you want to use big endian output.</param>
        /// <param name="NeededFormatStringToRecover">Variable to place an 'Unpack'-compatible format string into.</param>
        /// <returns>A Byte array containing the objects provided in binary format.</returns>
        public static byte[] Pack(object[] items, bool LittleEndian, out string NeededFormatStringToRecover)
        {

            // make a byte list to hold the bytes of output
            List<byte> outputBytes = new List<byte>();

            // should we be flipping bits for proper endinanness?
            bool endianFlip = (LittleEndian != BitConverter.IsLittleEndian);

            // start working on the output string
            string outString = (LittleEndian == false ? ">" : "<");

            // convert each item in the objects to the representative bytes
            foreach (object o in items)
            {
                byte[] theseBytes = TypeAgnosticGetBytes(o);
                if (endianFlip == true) theseBytes = (byte[])theseBytes.Reverse();
                outString += GetFormatSpecifierFor(o);
                outputBytes.AddRange(theseBytes);
            }

            NeededFormatStringToRecover = outString;

            return outputBytes.ToArray();

        }

        public static byte[] Pack(object[] items)
        {
            string dummy = "";
            return Pack(items, true, out dummy);
        }
    }
}
