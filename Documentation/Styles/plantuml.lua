-- Generates PlantUML images and injects global config
local pandoc = require 'pandoc'
local system = require 'pandoc.system'

function CodeBlock(block)
    if block.classes:includes('plantuml') then
        local content = block.text
        -- 1. Read Global Config
        local f = io.open('D:/UltimateStorage/GitSynced/SEP3/Documentation/Styles/plantuml-config.puml', 'r')
        local config = ''
        if f then config = f:read('*all'); f:close() end
        -- 2. Inject Config
        if content:find('@startuml') then
             content = content:gsub('@startuml', '@startuml\n' .. config)
        else
             content = '@startuml\n' .. config .. '\n' .. content .. '\n@enduml'
        end
        -- 3. Render
        local hash = pandoc.sha1(content)
        local img_dir = 'plantuml-images'
        pcall(system.make_directory, img_dir)
        local fname = img_dir .. '/' .. hash .. '.png'
        local f_img = io.open(fname, 'r')
        if f_img == nil then
            local result = pandoc.pipe('plantuml', {'-pipe', '-tpng'}, content)
            local f_out = io.open(fname, 'wb'); f_out:write(result); f_out:close()
        else
            f_img:close()
        end
        -- 4. Attributes
        local img_attr = pandoc.Attr(block.identifier, {}, {})
        if block.attributes['width'] then img_attr.attributes['width'] = block.attributes['width'] end
        local caption = block.attributes['caption']
        local img = pandoc.Image({}, fname, '', img_attr)
        return pandoc.Para({ img })
    end
end
