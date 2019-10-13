<?php

namespace App\Model;

use Illuminate\Database\Eloquent\Model;

class CategoryTreePath extends Model
{
    //
    //$var $ancestor');
    //$table->integer('descendant');
    //$table->integer('distance');
    protected $fillable = ['ancestor', 'descendant', 'distance','seq'];


    public function ancestorCategory()
    {
        return $this->belongsTo('App\Model\Category', "ancestor");
    }

    public function descendantCategory()
    {
        return $this->belongsTo('App\Model\Category', "descendant");
    }
}
