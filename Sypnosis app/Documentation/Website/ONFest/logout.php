<?php
/**
 * Created by PhpStorm.
 * User: KeDon
 * Date: 2018/5/28
 * Time: 12:40
 */
session_start();
session_unset();
session_destroy();
header("Location: index.php");
?>