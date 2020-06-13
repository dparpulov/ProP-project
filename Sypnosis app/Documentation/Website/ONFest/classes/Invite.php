<?php
require_once('DatabaseConnection.php');
class Invite
{
    public function  InvitePeople($email,$accountId)
    {

        if($this->CheckUser($email,$accountId)!=false&&$this->CheckEmail($email)!=null)
        {
            if($this->GetTicketId($this->CheckEmail($email))!=null&&$this->CheckOwner($email)!=true)
            {
                if (!empty($email) && !empty($accountId))
                {
                    $dc = new DatabaseConnection();
                    $pdo = $dc->Connect();

                    //insert info
                    try {
                        /* Create query to execute with placeholders */
                        $query = $pdo->prepare("INSERT INTO camper (ticket_id,campspot_id) VALUES (?,?)");
                        $query->execute([$this->GetTicketId($this->CheckEmail($email)), $this->GetCampspotId($accountId)]);
                        $queryUpdate = $pdo->prepare("UPDATE account SET balance = balance - 20 WHERE email = ?");
                        $queryUpdate->execute([$email]);

                        return true;
                    } catch (PDOException $e) {
                        echo $e->getMessage();
                    }
                }
            }
        }
    }
    public function CheckUser($email,$accountId){
        if (!empty($email) && !empty($accountId))
        {
            if($this->GetTicketId($accountId)!=null)
            {
                $dc = new DatabaseConnection();
                $pdo = $dc->Connect();

                try {
                    /* Create query to execute with placeholders */
                    $query = $pdo->prepare("SELECT reserved_by_id FROM campspot WHERE reserved_by_id = ?");
                    $query->execute([$this->GetTicketId($accountId)]);
                    $userRow = $query->fetch(PDO::FETCH_ASSOC);

                    if ($query->rowCount() > 0) {
                        return true;
                    }
                } catch (PDOException $e) {
                    echo $e->getMessage();
                }
                return false;
            }
        }
    }
    public  function  CheckEmail($email)
    {
        if (!empty($email))
        {
            $dc = new DatabaseConnection();
            $pdo = $dc->Connect();

            try {
                /* Create query to execute with placeholders */
                $query = $pdo->prepare("SELECT id FROM account WHERE email = ?");
                $query->execute([$email]);
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
    public function  CheckOwner($email)
    {
        $ticketId = $this->GetTicketId($this->CheckEmail($email));

        $dc = new DatabaseConnection();
        $pdo = $dc->Connect();
        try {
            /* Create query to execute with placeholders */
            $query = $pdo->prepare("SELECT reserved_by_id FROM campspot WHERE reserved_by_id = ?");
            $query->execute([$ticketId]);
            $userRow = $query->fetch(PDO::FETCH_ASSOC);

            if ($query->rowCount() > 0) {
                return true;
            }
        } catch (PDOException $e) {
            echo $e->getMessage();
        }
        return false;
    }
    public function GetTicketId($accountId)
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
    public  function GetCampspotId($accountId)
    {
        if (!empty($accountId)) {
            /* Create database connection */
            $dc = new DatabaseConnection();
            $pdo = $dc->Connect();

            try {
                /* Create query to execute with placeholders */
                $query = $pdo->prepare("SELECT id FROM campspot WHERE reserved_by_id = ?");
                $query->execute([$this->GetTicketId($accountId)]);
                $userRow = $query->fetch(PDO::FETCH_ASSOC);

                if ($query->rowCount() > 0) {
                    return $userRow['id'];
                }
            } catch (PDOException $e) {
                echo $e->getMessage();
            }
            return '';
        }
    }
    public function CheckCampspot($accountId)
    {
        if (!empty($accountId)) {
            /* Create database connection */
            $dc = new DatabaseConnection();
            $pdo = $dc->Connect();
//$this->GetCampspotId($accountId)
            try {
                /* Create query to execute with placeholders */
                $query = $pdo->prepare("SELECT * FROM camper WHERE campspot_id = ?");
                $query->execute([$this->GetCampspotId($accountId)]);
                $userRow = $query->fetch(PDO::FETCH_ASSOC);

                if ($query->rowCount() > 5) {
                    return false;
                }
            } catch (PDOException $e) {
                echo $e->getMessage();
            }
            return true;
        }
    }
    public function UpdateBalance($email,$accountId)
    {
        if (!empty($email)) {
            /* Create database connection */
            $dc = new DatabaseConnection();
            $pdo = $dc->Connect();
            try {
                /* Create query to execute with placeholders */
                $query = $pdo->prepare("INSERT ");
                $query->execute([$this->GetCampspotId($accountId)]);
                $userRow = $query->fetch(PDO::FETCH_ASSOC);

                if ($query->rowCount() > 5) {
                    return false;
                }
            } catch (PDOException $e) {
                echo $e->getMessage();
            }
            return true;
        }
    }


}
