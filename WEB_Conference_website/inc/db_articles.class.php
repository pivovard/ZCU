<?php

class db_articles extends db_pdo
{
    
    public function GetAllArticles()
    {
        $table_name = "articles";
        $columns = "*";
        $where = array();
        
        $result = $this->DBSelectAll($table_name, $columns, $where);
        
        return $result;
    }
    
    public function GetUserArticles($userID)
    {
        $table_name = "articles";
        $columns = "*";
        $where = array();
        $where[] = array("column" => "author", "value" => $userID, "symbol" => "=");
        
        $result = $this->DBSelectAll($table_name, $columns, $where);
        
        return $result;
    }
    
    public function GetRevArticle($scoreID)
    {
        $table_name = "articles";
        $columns = "*";
        $where = array();
        $where[] = array("column" => "score1", "value" => $scoreID, "symbol" => "=");
        $where[] = array("column" => "score2", "value" => $scoreID, "symbol" => "=");
        $where[] = array("column" => "score3", "value" => $scoreID, "symbol" => "=");
        
        $result = $this->DBSelectOneOR($table_name, $columns, $where);
        
        return $result;
    }
    
    public function GetOneArticle($articleID)
    {
        $table_name = "articles";
        $columns = "*";
        $where = array();
        $where[] = array("column" => "ID", "value" => $articleID, "symbol" => "=");
        
        $result = $this->DBSelectOne($table_name, $columns, $where);
        
        return $result;
    }
    
    public function ArticleTitleExists($title)
    {
        $table_name = "articles";
        $columns = "*";
        $where = array();
        $where[] = array("column" => "title", "value" => $title, "symbol" => "=");
        
        $result = $this->DBSelectOne($table_name, $columns, $where);
        
        return $result;
    }
    
    public function ArticleURLExists($URL)
    {
        $table_name = "articles";
        $columns = "*";
        $where = array();
        $where[] = array("column" => "URL", "value" => $URL, "symbol" => "=");
        
        $result = $this->DBSelectOne($table_name, $columns, $where);
        
        return $result;
    }
    
    public function AddArticle($article, $URL)
    {
        $res1 = $this->ArticleTitleExists($article["title"]);
        $res2 = $this->ArticleURLExists($URL);
        if(is_array($res1) || is_array($res2))
        {
            echo "Article or file already exists!";
            return null;
        }
        
        $table_name = "score";
        $item = array();
        
        $res1 = $this->DBInsert($table_name, $item);
        $res2 = $this->DBInsert($table_name, $item);
        $res3 = $this->DBInsert($table_name, $item);
        
        $table_name = "articles";
        $item = array("title" => $article["title"], "author" => $_SESSION["conference_system"]["ID"], "abstract" => $article["abstract"], "URL" => $URL, "score1" => $res1, "score2" => $res2, "score3" => $res3);
        
        $result = $this->DBInsert($table_name, $item);
        
        return $result;
    }
    
    public function UpdateArticle($articleID, $article, $URL)
    {
        $res = $this->ArticleURLExists($URL);
        if(is_array($res))
        {
            echo "File already exists!";
            return null;
        }
        
        $table_name = "articles";
        $item = array("abstract" => $article["abstract"], "URL" => $URL);
        $where = array();
        $where[] = array("column" => "ID", "value" => $articleID, "symbol" => "=");
        
        $result = $this->DBUpdate($table_name, $item, $where);
        
        return $result;
    }
    
    public function ApproveArticle($articleID)
    {
        $table_name = "articles";
        $item = array("pass" => 1);
        $where = array();
        $where[] = array("column" => "ID", "value" => $articleID, "symbol" => "=");
        
        $result = $this->DBUpdate($table_name, $item, $where);
        
        return $result;
    }
    
    public function DeleteArticle($articleID)
    {
        $article = $this->GetOneArticle($articleID);
        
        $table_name = "score";
        $where = array();
        $where[] = array("column" => "ID", "value" => $article["score1"], "symbol" => "=");
        $where[] = array("column" => "ID", "value" => $article["score2"], "symbol" => "=");
        $where[] = array("column" => "ID", "value" => $article["score3"], "symbol" => "=");
        $this->DBDeleteOR($table_name, $where);
        
        $table_name = "articles";
        $where = array();
        $where[] = array("column" => "ID", "value" => $articleID, "symbol" => "=");
        
        $result = $this->DBDelete($table_name, $where);
        
        return $result;
    }
    
    public function GetUser($userID)
    {
        $table_name = "users";
        $columns = "*";
        $where = array();
        $where[] = array("column" => "ID", "value" => $userID, "symbol" => "=");
        
        $result = $this->DBSelectOne($table_name, $columns, $where);
        
        return $result;
    }
    
    public function GetAllUsers()
    {
        $table_name = "users";
        $columns = "*";
        $where = array();
        
        $result = $this->DBSelectAll($table_name, $columns, $where);
        
        return $result;
    }
    
    public function GetReviewers()
    {
        $table_name = "users";
        $columns = "*";
        $where = array();
        $where[] = array("column" => "right", "value" => "3", "symbol" => "=");
        
        $result = $this->DBSelectAll($table_name, $columns, $where);
        
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
    
    public function SetRight($userID, $right)
    {
        $table_name = "users";
        $item = array("right" => $right);
        $where = array();
        $where[] = array("column" => "ID", "value" => $userID, "symbol" => "=");
        
        $result = $this->DBUpdate($table_name, $item, $where);
    }
    
    public function DeleteUser($userID)
    {
        $articles = $this->GetUserArticles($userID);
        foreach($articles as $art)
        {
            $this->DeleteArticle($art["ID"]);
        }
        
        $table_name = "users";
        $where = array();
        $where[] = array("column" => "ID", "value" => $userID, "symbol" => "=");
        
        $result = $this->DBDelete($table_name, $where);
        return $result;
    }
    
    public function SetReviewer($scoreID, $rev)
    {
        $table_name = "score";
        $item = array("rev" => $rev);
        $where = array();
        $where[] = array("column" => "ID", "value" => $scoreID, "symbol" => "=");
        
        $result = $this->DBUpdate($table_name, $item, $where);
        
        return $result;
    }
    
    public function GetScore($scoreID)
    {
        $table_name = "score";
        $columns = "*";
        $where = array();
        $where[] = array("column" => "ID", "value" => $scoreID, "symbol" => "=");
        
        $result = $this->DBSelectOne($table_name, $columns, $where);
        
        return $result;
    }
    
    public function UpdateScore($scoreID, $score)
    {
        $table_name = "score";
        $item = array("score1" => $score["score1"], "score2" => $score["score2"], "score3" => $score["score3"]);
        $where = array();
        $where[] = array("column" => "ID", "value" => $scoreID, "symbol" => "=");
        
        $result = $this->DBUpdate($table_name, $item, $where);
        
        return $result;
    }
    
    public function GetRevScores($userID)
    {
        $table_name = "score";
        $columns = "*";
        $where = array();
        $where[] = array("column" => "rev", "value" => $userID, "symbol" => "=");
        
        $result = $this->DBSelectAll($table_name, $columns, $where);
        
        return $result;
    }
    
    public function GetOverallScore($score1, $score2, $score3)
    {
        $score = $this->GetScore($score1);
        $result = $score["score1"] + $score["score2"] + $score["score3"];
        
        $score = $this->GetScore($score2);
        $result = $result + $score["score1"] + $score["score2"] + $score["score3"];
        
        $score = $this->GetScore($score3);
        $result = $result + $score["score1"] + $score["score2"] + $score["score3"];
        
        return $result;
    }
    
}

?>