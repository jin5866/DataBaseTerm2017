<?php
    $host = 'localhost';
    $user = 'root';
    $pw = '';
    $dbName = 'localhost';

    $ID = $_POST['ID'];

    mysql_connect($host, $user, $pw);
    mysql_select_db($dbName);
    $uniqueNumber = mysql_query("select UniqueNumber from customer where ID = \"$ID\"");
    $UN = mysql_fetch_array($uniqueNumber);
    $highcralist = mysql_query("select CrackerUN from cuscrarelation where CustomerUN = {$UN[UniqueNumber]} and Preference > 80 order by rand() limit 2" );
    $conscralist = array();
?>

<!DOCTYPE html>
<html>
    <head>
        <title>DBTerm</title>
        <link rel="stylesheet" type="text/css" href="/css/style.css"/>
        <script src = "http://code.jquery.com/jquery-1.10.2.js"></script>
        <script>
            $(document).ready(function() {
                $('.data_card').mouseenter(function() {
                    $(this).animate({
                    height: '+=10px'
                    });
                });
                $('.data_card').mouseleave(function() {
                    $(this).animate({
                    height: '-=10px'
                    }); 
                });
                $('.data_card').click(function() {
                    $(this).toggle(1000);
                }); 
            });
        </script>
    </head>
    <body>
        <div class = "logo">
            <a href="/index.php">
                <img src="/img/Logo.jpeg" alt="로고" style="max-width: 100%; height: auto;" >
            </a>
        </div>
        <div >
            <table>
                <tr>
                    <td class="menu"><a href="/index.php">메인 메뉴</a></td>
                    <td class="menu"><a href="/customer_list.php">유저 목록</a></td>
                    <td class="menu"><a href="/cracker_list.php">과자 목록</a></td>
                </tr>
            </table>
        </div>
        <div class = "card_list">
            <?php
            /*
            while($tmp = mysql_fetch_array($highcralist))
            {
                $craUN = $tmp["CrackerUN"];
                echo"<div class = \"data_card\"><img src=\"/img/cracker/$craUN.jpeg\" alt=\"$craUN사진\" style = \"max-width:170px; max-height:200px\"/></div>"; 
            }
            */
            while($tmp = mysql_fetch_array($highcralist))
            {
                $aa = mysql_query("select CrackerUN1 as CUN from cracrarelation where similarity > 55 and CrackerUN2 = {$tmp[CrackerUN]} order by rand() limit 2");
                while($a = mysql_fetch_array($aa))
                {
                    $craUN = $a["CUN"];
                    echo"<div class = \"data_card\"><img src=\"/img/cracker/$craUN.jpeg\" alt=\"$craUN사진\" style = \"max-width:170px; max-height:200px\"/><p>유사한 과자를 좋아하는 사람이 좋아함</p> </div>";
                }

                $bb = mysql_query("select CrackerUN2 as CUN from cracrarelation where similarity > 55 and CrackerUN1 = {$tmp[CrackerUN]} order by rand() limit 2");
                while($b = mysql_fetch_array($bb))
                {
                    $craUN = $b["CUN"];
                    echo"<div class = \"data_card\"><img src=\"/img/cracker/$craUN.jpeg\" alt=\"$craUN사진\" style = \"max-width:170px; max-height:200px\"/><p>유사한 과자를 좋아하는 사람이 좋아함</p></div>";
                }
                
            }

            $highcuslist = mysql_query("select CustomerUN2 as UniqueNumber from cuscusrelation where CustomerUN1 = {$UN[UniqueNumber]} and similarity > 60 order by rand() limit 2");

            while($tmp = mysql_fetch_array($highcuslist))
            {
                $cc = mysql_query("select CrackerUN as CUN from cuscrarelation where Preference > 80 and CustomerUN = {$tmp["UniqueNumber"]} order by rand() limit 2");
                while($c = mysql_fetch_array($cc))
                {
                    $craUN = $c["CUN"];
                    echo"<div class = \"data_card\"><img src=\"/img/cracker/$craUN.jpeg\" alt=\"$craUN사진\" style = \"max-width:170px; max-height:200px\"/><p>나와 선호가 유사한 사람이 좋아함</p></div>";
                }
                
            }

            $highcuslist = mysql_query("select CustomerUN1 as UniqueNumber from cuscusrelation where CustomerUN2 = {$UN[UniqueNumber]} and similarity > 60 order by rand() limit 2");

            while($tmp = mysql_fetch_array($highcuslist))
            {
                $dd = mysql_query("select CrackerUN as CUN from cuscrarelation where Preference > 80 and CustomerUN = {$tmp["UniqueNumber"]} order by rand() limit 2");
                while($d = mysql_fetch_array($dd))
                {
                    $craUN = $d["CUN"];
                    echo"<div class = \"data_card\"><img src=\"/img/cracker/$craUN.jpeg\" alt=\"$craUN사진\" style = \"max-width:170px; max-height:200px\"/><p>나와 선호가 유사한 사람이 좋아함</p></div>";
                }
                
            }
            /*
                while(($list = array_pop($conscralist))!=null) 
                {
                    $craUN = $list["CUN"];
                    echo"<div class = \"data_card\"><img src=\"/img/cracker/$craUN.jerg\" alt=\"$craUN사진\"/></div>";
                }*/
            ?>
            
        </div>
    </body>
</html>

    
