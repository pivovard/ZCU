<?php

class db_login extends db_pdo
{
    
    public function Login($user)
    {
        $table_name = "users";
        $columns = "*";
        $where = array();
        $where[] = array("column" => "login", "value" => $user["login"], "symbol" => "=");
        
        $result = $this->DBSelectOne($table_name, $columns, $where);
        
        return $result;
    }
    
    public function GetRight($rightID)
    {
        $table_name = "rights";
        $columns = "*";
        $where = array();
        $where[] = array("column" => "ID", "value" => $rightID, "symbol" => "=");
        
        $result = $this->DBSelectOne($table_name, $columns, $where);
        
        return $result;
    }
    
    public function Register($user)
    {
        $res = $this->Login($user);
        if(is_array($res)) return null;
        
        $table_name = "users";
        $item = array("login" => $user["login"], "pass" => $user["pass"], "mail" => $user["mail"], "right" => $user["right"]);
        
        $result = $this->DBInsert($table_name, $item);
        
        return $result;
    }
}

?>