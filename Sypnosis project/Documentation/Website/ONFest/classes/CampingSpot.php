<?php
require_once('DatabaseConnection.php');

class CampingSpot
{
    private $campspot;

    public function __construct()
    {
    }

    public function GetCampSpot()
    {

        /* Create database connection */
        $dc = new DatabaseConnection();
        $pdo = $dc->Connect();
        if ($pdo != null){
            try {
                /* Create query to execute with placeholders */
                $query = $pdo->prepare("SELECT * FROM `campspot` WHERE reserved_by_id IS NULL;");
                $query->execute();

                $campArray = $query->fetchAll();
                if ($query->rowCount() > 0) {
                    return $campArray;
                }
            } catch (PDOException $e) {
                echo $e->getMessage();
            }
        }
    }

}
