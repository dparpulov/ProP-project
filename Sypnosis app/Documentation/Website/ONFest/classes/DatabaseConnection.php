<?php

class DatabaseConnection
{
    private $host;
    private $db;
    private $user;
    private $pass;


    const HERA_CREDENTIALS = [
        'host'  => 'studmysql01.fhict.local',
        'db'    => 'dbi377952',
        'user'  => 'dbi377952',
        'pass'  => '1234',
    ];

    public function Connect($cred = DatabaseConnection::HERA_CREDENTIALS)
    {
        $this->host = $cred['host'];
        $this->db = $cred['db'];
        $this->user = $cred['user'];
        $this->pass = $cred['pass'];

        try {
            $pdo = new PDO("mysql:host=$this->host;dbname=$this->db", $this->user, $this->pass);
            $pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
            return $pdo;
        } catch (PDOException $e) {
            echo $e->getMessage();
        }
    }
}