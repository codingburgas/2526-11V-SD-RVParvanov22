import Sidebar from '../components/home/Sidebar'
import FeedComposer from '../components/home/FeedComposer'
import PostCard from '../components/home/PostCard'
import RightPanel from '../components/home/RightPanel'

function HomePage() {
  return (
    <div className="home-page">
      <div className="home-container">
        {/* Left Sidebar */}
        <div className="home-sidebar-wrapper">
          <Sidebar />
        </div>

        {/* Center Feed */}
        <div className="home-feed-wrapper">
          {/* Feed Header */}
          <div className="feed-header mb-4">
            <h1 className="feed-title fw-bold">Home Feed</h1>
            <p className="feed-subtitle text-light opacity-75">
              Posts, clips, achievements and recent matches from the gaming community.
            </p>
          </div>

          {/* Create Post Composer */}
          <FeedComposer />

          {/* Sample Posts */}
          <PostCard 
            username="RumenXP"
            handle="@rumenxp"
            avatar="https://api.dicebear.com/7.x/avataaars/svg?seed=rumen"
            game="Valorant"
            rank="Immortal 3"
            timestamp="2h ago"
            postType="Achievement"
            content="Reached Immortal 3 after winning 6 of my last 7 games. Looking for a serious team to scrim with this weekend."
            stats={{
              lastMatch: '✓ Win',
              kda: '24/15/8',
              map: 'Ascent'
            }}
            likes={342}
            comments={28}
            views={1240}
          />

          <PostCard 
            username="ClutchNova"
            handle="@clutchnova"
            avatar="https://api.dicebear.com/7.x/avataaars/svg?seed=clutch"
            game="CS2"
            rank="Global Elite"
            timestamp="4h ago"
            postType="Match Result"
            content="Top fragged on Mirage and finally hit Faceit Level 8 🎯 GG to my team, clutch round at 14-14!"
            stats={{
              lastMatch: '✓ Win',
              kda: '28/12/6',
              map: 'Mirage'
            }}
            mediaType="image"
            likes={512}
            comments={45}
            views={2150}
          />

          <PostCard 
            username="Vexara"
            handle="@vexara_plays"
            avatar="https://api.dicebear.com/7.x/avataaars/svg?seed=vexara"
            game="League of Legends"
            rank="Diamond I"
            timestamp="6h ago"
            postType="Clip"
            content="Promoted to Diamond and uploaded a clip from today's ranked session. Insane 1v5 teamfight as Ahri mid!"
            mediaType="video"
            likes={867}
            comments={73}
            views={3890}
          />

          <PostCard 
            username="EliteGamer"
            handle="@elite_gamer"
            avatar="https://api.dicebear.com/7.x/avataaars/svg?seed=elite"
            game="Valorant"
            rank="Ascendant 2"
            timestamp="8h ago"
            postType="Team Search"
            content="Looking for a competitive team for the upcoming VCT open qualifiers. Sentinel/Initiator main with 2+ years of organized play experience. DM if interested!"
            likes={234}
            comments={18}
            views={890}
          />
        </div>

        {/* Right Panel */}
        <div className="home-right-wrapper">
          <RightPanel />
        </div>
      </div>
    </div>
  )
}

export default HomePage