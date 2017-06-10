<?php
    $host = 'localhost';
    $user = 'root';
    $pw = '';
    $dbName = 'localhost';

    mysql_connect($host, $user, $pw);
    mysql_select_db($dbName);
    $list = mysql_query("select UniqueNumber, name, size from cracker");
?>

<!DOCTYPE html>
<html>
    <head>
        <title>DBTerm</title>
        <link rel="stylesheet" type="text/css" href="/css/style.css"/>
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
        <div class = "search">
            <form style = "text-align : center" class="form-inline" action=list.php method="post">
                <input type="text" class="" name="ID" placeholder="아이디 검색">
                <button type="submit" class="">검색</button>
            </form>
        </div>
        <table class ="list">
            <tr>
                <th class = "list_table">번호</th>
                <th class = "list_table">이름</th>
                <th class = "list_table">사진</th>
            </tr>        
            
            <?php
                while($row = mysql_fetch_array($list)){
                    $number = $row["UniqueNumber"];
                    $name = $row["name"];

                    echo"<tr>";
                    echo"<td class = \"list_table\">$number</td>";
                    echo"<td class = \"list_table\">$name</td>";
                    echo"<td class = \"list_table\"><img src=\"/img/cracker/$number.jpeg\" alt=\"$name\" style = \"max-width: 150px; height: auto;\" /></td>";
                    echo"</tr>";
                }
            ?>
        </table>
    </body>
</html>

    
