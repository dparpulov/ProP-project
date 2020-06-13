<?php
require_once('DatabaseConnection.php');

class Ticket
{
    public function Purchase($accountId, $campId)
    {
        $dc = new DatabaseConnection();
        $pdo = $dc->Connect();
        //only purchase ticket
        $ticketId = rand(1, 9999999);
        //check if you already have a ticket
        $query = $pdo->prepare("SELECT * FROM ticket WHERE account_id = ?");
        $query->execute([$accountId]);

        $query->fetch(PDO::FETCH_ASSOC);

        $hasTicket = false;

        if ($query->rowCount() > 0) {
            $hasTicket = true;
        }
        if (!$hasTicket) {
            $sql = "SELECT * FROM ticket where id = ?";
            $statement1 = $pdo->prepare($sql);
            $statement1->execute([$ticketId]);
            $row_nr1 = $statement1->rowCount();
            if ($row_nr1 > 0) {
                $ticketId = rand(1, 9999999);
            }

            if (!empty($accountId) && ($campId == "Null")) {
                $dc = new DatabaseConnection();
                $pdo = $dc->Connect();

                try {
                    /* Create query to execute with placeholders */
                    $query = $pdo->prepare("INSERT INTO ticket (account_id,id) VALUES (?,?)");
                    $query->execute([$accountId, $ticketId]);
                    $queryUpdate = $pdo->prepare("UPDATE account SET balance = balance - 55 WHERE id = ?");
                    $queryUpdate->execute([$accountId]);

//                    echo '<div class="alert alert-success" role="alert">A simple success alert—check it out!</div>';
                    return true;
                } catch (PDOException $e) {
                    echo $e->getMessage();
                }
            }
            //purchase camping spot and ticket
            if (!empty($accountId) && ($campId != "Null")) {
                //get ticket id
                $dc = new DatabaseConnection();
                $pdo = $dc->Connect();

                //insert info
                try {
                    /* Create query to execute with placeholders */
                    $query = $pdo->prepare("INSERT INTO ticket (account_id,id) VALUES (?,?)");
                    $query->execute([$accountId, $ticketId]);
                    $query = $pdo->prepare("INSERT INTO camper (campspot_id,ticket_id) VALUES (?,?)");
                    $query->execute([$campId, $ticketId]);
                    $query = $pdo->prepare("UPDATE campspot SET reserved_by_id = ? WHERE id = ? ");
                    $query->execute([$ticketId,$campId]);
                    $queryUpdate = $pdo->prepare("UPDATE account SET balance = balance - 85 WHERE id = ?");
                    $queryUpdate->execute([$accountId]);
                    return true;
                } catch (PDOException $e) {
                    echo $e->getMessage();
                }

            } else {
                echo "Enter Name or Email Address";
            }
        }
        else{
//            echo'<div class="alert alert-danger" role="alert">Everyone only can buy one ticket!</div>';
        }
    }
    public function  ReserveCampingSpot($campId,$accountId)
    {
        $invite = new Invite();
        $ticketId=$invite->GetTicketId($accountId);
        if($ticketId!=null)
        {
            $dc = new DatabaseConnection();
            $pdo = $dc->Connect();

            //insert info
            try {
                /* Create query to execute with placeholders */
                $query = $pdo->prepare("INSERT INTO camper (campspot_id,ticket_id) VALUES (?,?)");
                $query->execute([$campId, $ticketId]);
                $queryUpdate = $pdo->prepare("UPDATE account SET balance = balance - 30 WHERE id = ?");
                $queryUpdate->execute([$accountId]);
                $query = $pdo->prepare("UPDATE campspot SET reserved_by_id = ? WHERE id = ? ");
                $query->execute([$ticketId,$campId]);

                return true;
            } catch (PDOException $e) {
                echo $e->getMessage();
            }
        }
    }

    public function GetTicketIdByUserId($accountId)
    {
        if (!empty($accountId)) {
            /* Create database connection */
            $dc = new DatabaseConnection();
            $pdo = $dc->Connect();

            try {
                /* Create query to execute with placeholders */
                $query = $pdo->prepare("SELECT id FROM ticket WHERE account_id = ?");
                $query->execute([$accountId]);
                $userRow = $query->fetch(PDO::FETCH_ASSOC);

                if ($query->rowCount() > 0) {
                    return $userRow['id'];
                }
            } catch (PDOException $e) {
                echo $e->getMessage();
            }
            return null;
        }
    }
}