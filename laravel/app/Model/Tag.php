<?php

namespace App\Model;

use Illuminate\Database\Eloquent\Model;

class Tag extends Model
{
    //
    public function post()
    {
        return $this->belongsToMany('App\Model\Post');
    }
}