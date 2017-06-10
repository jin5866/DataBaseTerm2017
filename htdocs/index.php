<?php
    $host = 'localhost';
    $user = 'root';
    $pw = '';
    $dbName = 'localhost';

    mysql_connect($host, $user, $pw);
    mysql_select_db($dbName);
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
        <div class ="list">
            <h1>맛있는 과자를 먹어봅시다</h1>
        </div>
    </body>
</html>

    
