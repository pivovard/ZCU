<?php
global $key;

$subpage = @$_REQUEST["subpage"]."";
$action = @$_REQUEST["action"]."";
$articleID = @$_REQUEST["articleID"]."";
$article = @$_POST["article"];
            
$db = new db_articles();
$db->Connect();
            
if($action == "add")
{
    $res = $db->AddArticle($article, $_FILES["file"]["name"]);
    
    if(is_numeric($res))
    {
        $resUp = FileUpload();
        if($resUp)
        {
            header("Location:index.php?page=author");
        }
        else
        {
            $db->DeleteArticle($res);
        }
    }
}
            
if($action == "update")
{
    $res = $db->UpdateArticle($articleID, $article, $_FILES["file"]["name"]);
    
    if(is_numeric($res))
    {
        $resUp = FileUpload();
        if($resUp)
        {
            header("Location:index.php?page=author");
        }
    }
}
            
if($action == "delete")
{
    $db->DeleteArticle($articleID);
}
            
if($subpage == "add")
{
    echo "<form class='form-horizontal' action='index.php?page=author&subpage=add' method='post' enctype='multipart/form-data'>
        <input type='hidden' name='action' value='add'/>
        <div class='form-group'>
            <label class='control-label col-sm-2' for='title'>Title:</label>
            <div class='col-sm-8'>
                <input type='text' class='form-control' id='title' name='article[title]' placeholder='Enter title' required>
             </div>
        </div>
        <div class='form-group'>
            <label class='control-label col-sm-2' for='abstract'>Abstract:</label>
            <div class='col-sm-8'>
                <textarea type='text' class='form-control' rows='10' id='abstract' name='article[abstract]'></textarea>
             </div>
        </div>
        <div class='form-group'>
            <label class='control-label col-sm-2' for='file'>File:</label>
            <div class='col-sm-8'>
                <input type='file' id='file' name='file' required>
             </div>
        </div>";
        
        if($action == "add")
                    {
                        echo "
                            <div class='form-group'>
                                <label class='col-sm-offset-2 col-sm-8' style='color:red'>Article hasn't been added!</label>
                            </div>";
                    }
        echo "
        <div class='form-group'>
            <div class='col-sm-offset-2 col-sm-8'>
                <button type='submit' class='btn btn-info'>Add article</button>
             </div>
        </div>
        </form>";
}
else if(is_numeric($subpage))
{
    $article = $db->GetOneArticle($subpage);
    
    echo "<form class='form-horizontal' action='index.php?page=author&subpage=$subpage' method='post' enctype='multipart/form-data'>
        <input type='hidden' name='action' value='update'/>
        <input type='hidden' name='articleID' value='$article[ID]'/>
        <div class='form-group'>
            <label class='control-label col-sm-2' for='title'>Title:</label>
            <div class='col-sm-8'>
                <input type='text' class='form-control' id='title' name='article[title]' placeholder='Enter title' value='$article[title]' disabled>
             </div>
        </div>
        <div class='form-group'>
            <label class='control-label col-sm-2' for='abstract'>Abstract:</label>
            <div class='col-sm-8'>
                <textarea type='text' class='form-control' rows='10' id='abstract' name='article[abstract]'>$article[abstract]</textarea>
             </div>
        </div>
        <div class='form-group'>
            <label class='control-label col-sm-2' for='file'>File:</label>
            <div class='col-sm-8'>
                <input type='file' id='file' name='file' required>
             </div>
        </div>";
    
        if($action == "update")
                    {
                        echo "
                            <div class='form-group'>
                                <label class='col-sm-offset-2 col-sm-8' style='color:red'>Article hasn't been updated!</label>
                            </div>";
                    }
    echo "
        <div class='form-group'>
            <div class='col-sm-offset-2 col-sm-8'>
                <button type='submit' class='btn btn-info'>Update article</button>
             </div>
        </div>
        </form>";
}
else
{
    echo "<table class='table table-hover'>
                <thead>
                    <tr>
                        <th>Title</th>
                        <th>Link</th>
                        <th>Score</th>
                        <th>Approved</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>";
    
    $articles = $db->GetUserArticles($_SESSION[$key]["ID"]);
                    
    foreach($articles as $art)
    {
        $score = $db->GetOverallScore($art["score1"], $art["score2"], $art["score3"]);
        if($art["pass"] == 0) $pass = "NO";
        else $pass = "YES";
    
        echo "<tr><td><a href='index.php?page=author&subpage=$art[ID]'>$art[title]</a></td>
            <td><a href='pdf/$art[URL]'>$art[URL]</a></td>
            <td>$score</td>
            <td>$pass</td>
            <td><a href='index.php?page=author&action=delete&articleID=$art[ID]' class='glyphicon glyphicon-remove' style='color:red'></a></td></tr>";
    }
    
    echo "</tbody>
            </table>
            <a href='index.php?page=author&subpage=add' class='btn btn-info' role='button'>New article</a>";
}
        
        
?>            
        