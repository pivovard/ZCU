<?php

class predmety extends db_pdo
{
    public function LoadAll()
    {
        $table_name = "madostal_predmety2";
        $columns = "*";
        $where = array();
        $predmety = $this->DBSelectAll($table_name, $columns, $where);
        return $predmety;
    }
    
    public function LoadOne()
    {
        $table_name = "madostal_predmety2";
        $columns = "*";
        $where = array();
        $where[] = array("column" => "zkratka", "value" => "KIV/DB1", "symbol" => "=");
        $predmety = $this->DBSelectAll($table_name, $columns, $where);
        return $predmety;
    }
}