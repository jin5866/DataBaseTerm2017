<?php
    $url = "http://" . $_SERVER["HTTP_HOST"] . $_SERVER["REQUEST_URI"]; 
    $url = urlencode($url); 
    define("URL",$url);
?>

<!DOCTYPE html>
<html>
    <head>
        <title>DBTerm</title>
        <link type="text/css" href="/css/tyle.css">
    </head>
    <body>
        <div class = "logo">
            <a href="/index.php">
                <img src="/img/Logo.jpeg" alt="로고" style="max-width: 20%; height: auto;" >
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
        <div>
            dkdkdk
        </div>
    </body>
</html>

    
