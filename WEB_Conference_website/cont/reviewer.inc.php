<?php 
global $key;
  
$subpage = @$_REQUEST["subpage"]."";
$action = @$_REQUEST["action"]."";
$score = @$_POST["score"];
            
$db = new db_articles();
$db->Connect();
            
if($action == "mark")
{
    //printr($score);
    $res = $db->UpdateScore($subpage, $score);
    
    if(is_numeric($res)) header("Location:index.php?page=reviewer");
}
            
if(is_numeric($subpage))
{
    $score = $db->GetScore($subpage);
    $article = $db->GetRevArticle($subpage);
    
    echo "<form class='form-horizontal' action='index.php?page=reviewer&subpage=$subpage' method='post'>
        <input type='hidden' name='action' value='mark'/>
        <div class='form-group'>
            <label class='control-label col-sm-2' for='title'>Title:</label>
            <div class='col-sm-8'>
                <input type='text' class='form-control' id='title' name='article[title]' placeholder='Enter title' value='$article[title]' disabled>
             </div>
        </div>
        <div class='form-group'>
            <label class='control-label col-sm-2' for='abstract'>Abstract:</label>
            <div class='col-sm-8'>
                <textarea type='text' class='form-control' rows='10' id='abstract' name='article[abstract]' disabled>$article[abstract]</textarea>
             </div>
        </div>
        <div class='form-group'>
            <label class='control-label col-sm-2' for='file'>File:</label>
            <label class='col-sm-8'>
                <a href='pdf/$article[URL]'>$article[URL]</a>
             </label>
        </div>";
    
    echo "<div class='form-group'>
            <label class='control-label col-sm-2' for='score1'>Score 1:</label>
            <label class='control-label'>$score[score1] </label>
            <div class='col-sm-2'>
                <select class='form-control' id='score1' name='score[score1]'>
                    <option value=''></option>
                    <option value='1'>1 - Good</option>
                    <option value='2'>2 - Average</option>
                    <option value='3'>3 - Bad</option>
                </select>
             </div>
        </div>
        <div class='form-group'>
            <label class='control-label col-sm-2' for='score2'>Score 2:</label>
            <label class='control-label'>$score[score2] </label>
            <div class='col-sm-2'>
                <select class='form-control' id='score2' name='score[score2]'>
                    <option value=''></option>
                    <option value='1'>1 - Good</option>
                    <option value='2'>2 - Average</option>
                    <option value='3'>3 - Bad</option>
                </select>
             </div>
        </div>
        <div class='form-group'>
            <label class='control-label col-sm-2' for='score3'>Score 3:</label>
            <label class='control-label'>$score[score3] </label>
            <div class='col-sm-2'>
                <select class='form-control' id='score3' name='score[score3]'>
                    <option value=''></option>
                    <option value='1'>1 - Good</option>
                    <option value='2'>2 - Average</option>
                    <option value='3'>3 - Bad</option>
                </select>
             </div>
        </div>";
    
        if($action == "mark")
                    {
                        echo "
                            <div class='form-group'>
                                <label class='col-sm-offset-2 col-sm-8' style='color:red'>Score hasn't been updated!</label>
                            </div>";
                    }
    echo "
        <div class='form-group'>
            <div class='col-sm-offset-2 col-sm-8'>
                <button type='submit' class='btn btn-info'>Mark article</button>
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
                        <th>Score 1</th>
                        <th>Score 2</th>
                        <th>Score 3</th>
                        <th>Sum</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>";
    
    $scores = $db->GetRevScores($_SESSION[$key]["ID"]);
    
    foreach($scores as $score)
    {
        $article = $db->GetRevArticle($score["ID"]);
        $sum = $score["score1"] + $score["score2"] + $score["score3"];
        
        echo "<tr><td><a href='index.php?page=reviewer&subpage=$score[ID]'>$article[title]</a></td>
            <td><a href='pdf/$article[URL]'>$article[URL]</a></td>
            <td>$score[score1]</td>
            <td>$score[score2]</td>
            <td>$score[score3]</td>
            <td>= $sum</td>";
    }
    
    echo "</tbody>
            </table>";
}
            
            
?>