Common = { }

UIType = {
    SingleUI = 0,
    GroupUI = 1,
}

GMsgType = {
    RoleDead = 1,
}

function log(str, col)
    UnityEngine.Debug.Log("<color=" .. col .. ">" .. tostring(str) .. "</color>")
end

function logBlue(str)
    log(str, "#36bbfd")
end

return Common