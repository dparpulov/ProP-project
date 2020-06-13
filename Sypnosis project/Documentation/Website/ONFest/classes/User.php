<?php
require_once('DatabaseConnection.php');

class User
{
    public function UserRegister($firstName,$lastName,$email, $password)
    {
        if (!empty($email) && !empty($password) && !empty($firstName) && !empty($lastName)) {
            /* Create database connection */
            $dc = new DatabaseConnection();
            $pdo = $dc->Connect();

            $sql="SELECT * FROM account where email=:email";
            $statement= $pdo->prepare($sql);
            $statement->execute(
                array(
                    ':email'  => $_POST['email']
                )
            );
            $row_nr=$statement->rowCount();
            if($row_nr>0)
            {
                echo '<script>alert("The email is existed. Please change a new email");window.history.go(-1);</script>';
            }

            try
            {
                $passwordHash = password_hash($password, PASSWORD_DEFAULT);
                /* Create query to execute with placeholders */
                $query = $pdo->prepare("INSERT INTO account (first_name,last_name,email,password)
                         VALUES (?,?,?,?)");
                $query->execute([$firstName, $lastName, $email, $passwordHash]);
                return true;
            }
            catch (PDOException $e)
            {
                echo $e->getMessage();
            }
        }
    }

    public function UserLogin($email, $password)
    {
        if (!empty($email) && !empty($password)) {
            /* Create database connection */
            $dc = new DatabaseConnection();
            $pdo = $dc->Connect();

            try {
                /* Create query to execute with placeholders */

                $query = $pdo->prepare("SELECT * FROM account WHERE email = ?");
                $query->execute([$email]);

                $userRow = $query->fetch(PDO::FETCH_ASSOC);


                if ($query->rowCount() > 0) {

                    /* Validate encrypted password */
                    //$passwordHash = password_verify($password, PASSWORD_DEFAULT);
                    if (password_verify($password, $userRow['password'])) {
                        $_SESSION['user_id'] = $userRow['id'];
                        $_SESSION['user_name'] = $userRow['first_name'] . ' ' . $userRow['last_name'];
                        $_SESSION['email']=$userRow['email'];
                        $_SESSION['balance']=$userRow['balance'];

                        return true;
                    } else {
                        return false;
                    }
                }
            } catch (PDOException $e) {
                echo $e->getMessage();
            }
        } else {
            echo "Enter username or password";
        }
    }

    public function Update($email)
    {
        $dc = new DatabaseConnection();
        $pdo = $dc->Connect();

        try {
            $query = $pdo->prepare("SELECT * FROM account WHERE email = ?");
            $query->execute([$email]);
            $userRow = $query->fetch(PDO::FETCH_ASSOC);
            if ($query->rowCount() > 0) {

                /* Validate encrypted password */
                //$passwordHash = password_verify($password, PASSWORD_DEFAULT);
                if (!empty($email)) {
                    $_SESSION['balance'] = $userRow['balance'];

                }
            }
        }catch (PDOException $e){
            echo $e->getMessage();
        }
    }

    public  function ChangePassword($oldpassword,$newpassword,$email){
        if (!empty($oldpassword) && !empty($newpassword) && !empty($email)) {
            /* Create database connection */
            $dc = new DatabaseConnection();
            $pdo = $dc->Connect();

            try {
                /* Create query to execute with placeholders */
                $query = $pdo->prepare("SELECT * FROM account WHERE email = ?");
                $query->execute([$email]);
                $userRow = $query->fetch(PDO::FETCH_ASSOC);

                if ($query->rowCount() > 0) {
                    $id = $userRow['id'];
                    /* Validate encrypted password */
                    if (password_verify($oldpassword, $userRow['password'])){
                        $passwordHash = password_hash($newpassword, PASSWORD_DEFAULT);
                        /* Create query to execute with placeholders */

                        $query = $pdo->prepare("UPDATE account SET password=:password WHERE id = :id ");
                        $query->execute(array(
                            ':password' => $passwordHash,
                            ':id' => $id
                        ));
                        return true;
                    }
                }
            }
            catch (PDOException $e) {
                echo $e->getMessage();
            }
        }
    }

    public function Deposit($amount,$email,$balance){
        if (!empty($amount) && !empty($balance) && !empty($email))     {
            /* Create database connection */
            $dc = new DatabaseConnection();
            $pdo = $dc->Connect();

            try {
                /* Create query to execute with placeholders */
                $query = $pdo->prepare("SELECT * FROM account WHERE email = ?");
                $query->execute([$email]);
                $userRow = $query->fetch(PDO::FETCH_ASSOC);

                if ($query->rowCount()>0){

                    $newBalance=$balance+$amount;
                    $query=$pdo->prepare("UPDATE account SET balance=:balance WHERE email=:email");
                    $query->execute(array(
                        ':balance'=>$newBalance,
                        ':email'=>$email

                    ));
                    $_SESSION['balance'] = $newBalance;
                    return true;
                }

            }catch (PDOException $e) {
                echo $e->getMessage();
            }
        }
    }

    public function Withdraw($amount,$email,$balance){
        if (!empty($amount) && !empty($balance) && !empty($email))     {
            /* Create database connection */
            $dc = new DatabaseConnection();
            $pdo = $dc->Connect();

            try {
                /* Create query to execute with placeholders */
                $query = $pdo->prepare("SELECT * FROM account WHERE email = ?");
                $query->execute([$email]);
                $userRow = $query->fetch(PDO::FETCH_ASSOC);

                if ($query->rowCount()>0){

                    $newBalance=$balance-$amount;
                    $query=$pdo->prepare("UPDATE account SET balance=:balance WHERE email=:email");
                    $query->execute(array(
                        ':balance'=>$newBalance,
                        ':email'=>$email

                    ));
                    $_SESSION['balance'] = $newBalance;
                    return true;
                }
            }catch (PDOException $e) {
                echo $e->getMessage();
            }
        }
    }

    public function GetBalance($email)
    {
        if (!empty($email)) {
            /* Create database connection */
            $dc = new DatabaseConnection();
            $pdo = $dc->Connect();

            try {
                /* Create query to execute with placeholders */
                $query = $pdo->prepare("SELECT * FROM account WHERE email = ?");
                $query->execute([$email]);
                $userRow = $query->fetch(PDO::FETCH_ASSOC);

                if ($query->rowCount() > 0) {
                    $balance=$userRow['balance'];
                    $query=$pdo->prepare("UPDATE account SET balance=:balance WHERE email=:email");

                    $query->execute(array(
                        ':balance'=>$balance,
                        ':email'=>$email
                    ));
                }
            } catch (PDOException $e) {
                echo $e->getMessage();
            }
        }
    }

    public function GetTransactionHistory($accountId)
    {
        if (!empty($accountId)) {
            /* Create database connection */
            $dc = new DatabaseConnection();
            $pdo = $dc->Connect();

            try {
                /* Create query to execute with placeholders */
                $query = $pdo->prepare("SELECT i.name, s.date_time, s.quantity, s.price FROM sale s JOIN item i ON (s.item_id = i.id) WHERE account_id = ? ORDER BY date_time");
                $query->execute([$accountId]);
                $transactionArray = $query->fetchAll();

                if ($query->rowCount() > 0) {
                    return $transactionArray;
                }
            } catch (PDOException $e) {
                echo $e->getMessage();
            }
            return null;
        }
    }

    //Upload Picture
    public function UploadPicture($imgFile,$email)
    {
        if (!empty($imgFile))
        {
            $dc = new DatabaseConnection();
            $pdo = $dc->Connect();
            //insert info
            try {
                /* Create query to execute with placeholders */
                $queryUpdate = $pdo->prepare("UPDATE account SET picture = ? WHERE email = ?");
				$queryUpdate->bindParam(1, $imgFile, PDO::PARAM_LOB);
				$queryUpdate->bindParam(2, $email);
                $queryUpdate->execute();

                return true;
            } catch (PDOException $e) {
                echo $e->getMessage();
            }
        }
    }
	
    //Download the Picture
    public function GetPictures($email)
    {
        /* Create database connection */
        $dc = new DatabaseConnection();
        $pdo = $dc->Connect();
        if ($pdo != null){
            try {
                /* Create query to execute with placeholders */
                $query = $pdo->prepare("SELECT picture FROM `account` WHERE picture IS NOT NULL AND email = ?;");
                $query->execute([$email]);

                $artistArray = $query->fetch();
                if ($query->rowCount() > 0) {
                    return $artistArray;
                };
            } catch (PDOException $e) {
                echo $e->getMessage();
            }
        }
    }

}