import iconHome from "@/assets/images/icon-home.svg"
import iconArchived from "@/assets/images/icon-archive.svg"
import { Logo } from "@/components/connexion/FormContainerSignIn"

export function SideBar() {

    return (
        <div className="flex flex-col gap-y-10 bg-neutral-0 border border-neutral-300 w-74 h-full">
            <div className="gap-y-5 px-5 pt-5 pb-2.5">
                <Logo />
            </div>
            <div className="flex flex-col gap-y-4 px-4 pb-5">
                <div>
                    <ItemNavigationSideBar icon={iconHome} text="Home" isSelected={true} />
                    <ItemNavigationSideBar icon={iconArchived} text="Archived" isSelected={false} />
                </div>
                <div>
                    <span className="h-5.25 items-center px-3 pb-1 text-[#34D4D4D] text-xs font-bold">TAGS</span>
                    <div>
                        <ContentItemNavigationSideBar text="AI" numberBadge={1} />
                        <ContentItemNavigationSideBar text="Community" numberBadge={5} />
                        <ContentItemNavigationSideBar text="Compatibility" numberBadge={1} />
                        <ContentItemNavigationSideBar text="CSS" numberBadge={6} />
                        <ContentItemNavigationSideBar text="Design" numberBadge={1} />
                    </div>
                </div>
            </div>
        </div>
    )
}

export function ItemNavigationSideBar({ icon, text, isSelected }: { icon: string, text: string, isSelected: boolean }) {

    return (
        <div className={`flex flex-row items-center gap-x-2 px-3 py-2 rounded-6 ${isSelected ? 'bg-neutral-100 border border-neutral-100 text-neutral-900' : 'text-neutral-800'}`}>
            <div className="flex flex-row items-center gap-x-2">
                <img src={icon} className="size-5" alt="icon navigation" />
                <span className="text-preset-3">{text}</span>
            </div>
        </div>
    )
}

export function ContentItemNavigationSideBar({ text, numberBadge }: { text: string, numberBadge: number }) {

    return (
        <div className="flex justify-between px-3 py-2.5">
            <div className="flex flex-row items-center gap-x-2">
                <input type="checkbox" />
                <span className="text-preset-3 text-neutral-800"> {text} </span>
            </div>
            <div className="items-center px-2 pb-0.5 rounded-full bg-neutral-100 border border-neutral-300">
                <span className="text-neutral-800 text-xs font-bold">{numberBadge}</span>
            </div>
        </div>
    )
}