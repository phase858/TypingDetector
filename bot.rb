require "discordrb"

@location = File.expand_path(File.dirname(__FILE__))

def logger(text)
  log = File.open(File.join(@location, "bot.log"), "a+")

  time = "[#{Time.now.strftime("%Y-%m-%dT%H:%M:%SZ%z")}] "

  puts time+text

  log.puts time+text

  log.close
end

@warns_folder = File.join(@location, "warns")

logger "Changing working directory to responses folder at: #{@warns_folder}"

Dir.chdir(@warns_folder)

logger "Looking for images"

@warns = Dir.glob("**/*.png")

@warns.each {|image| logger "Found image #{File.join(@warns_folder, image)}"}

def chooser()
  choice = @warns[rand(@warns.length)]

  return File.join(@warns_folder, choice)
end

bot = Discordrb::Bot.new token: "TOKEN HERE"

bot.message() do |event|
  processed = event.content.gsub(/(<@\d+>)/,"").gsub(" ","")

  unless event.user.bot_account?
    if processed.empty?
      logger "#{event.user.name}: \"#{event.content}\" > \"#{processed}\" resp: \"\""
    else
      response = chooser
      event.send_file(File.open(response, "r"), caption: event.user.mention)
      logger "#{event.user.name}: \"#{event.content}\" > \"#{processed}\" resp: \"#{response}\""
    end
  end
end

logger "Starting Bot"

bot.run
