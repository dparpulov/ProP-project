<?php
require_once('DatabaseConnection.php');

class Subscribe
{
    public function Sub($FirstName,$LastName,$Email){
        if(!empty($FirstName)&&!empty($LastName)&&!empty($Email)){
            /* Create database connection*/
            $dc = new DatabaseConnection();
            $pdo = $dc->Connect();
            try {
                /* Create query to execute with placeholders */
                $query = $pdo->prepare("INSERT INTO subscriber (email, first_name, last_name ) VALUES (?, ?, ?)");
                $query->execute([$Email,$FirstName, $LastName]);

                return true;
            } catch (PDOException $e) {
                echo $e->getMessage();
            }
        } else {
            echo "Enter Name or Email Address";
        }
    }

}
