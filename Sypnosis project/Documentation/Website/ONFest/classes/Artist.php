<?php
require_once('DatabaseConnection.php');

class Artist
{
    private $picture;

    public function __construct()
    {
    }

    public function GetPictures()
    {

        /* Create database connection */
        $dc = new DatabaseConnection();
        $pdo = $dc->Connect();
        if ($pdo != null){
            try {
                /* Create query to execute with placeholders */
                $query = $pdo->prepare("SELECT name, picture FROM artist");
                $query->execute();

                $artistArray = $query->fetchAll();
                if ($query->rowCount() > 0) {
                    return $artistArray;
                }
            } catch (PDOException $e) {
                echo $e->getMessage();
            }
        }
    }


}